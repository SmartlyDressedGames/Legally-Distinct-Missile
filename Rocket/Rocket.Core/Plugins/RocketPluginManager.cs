using Rocket.API;
using Rocket.Core.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using System.Linq;
using Rocket.Core.Extensions;

namespace Rocket.Core.Plugins
{
    public sealed class RocketPluginManager : MonoBehaviour
    {
        public delegate void PluginsLoaded();
        public event PluginsLoaded OnPluginsLoaded;

        private static List<Assembly> pluginAssemblies;
        private static List<GameObject> plugins = new List<GameObject>();
        internal static List<IRocketPlugin> Plugins { get { return plugins.Select(g => g.GetComponent<IRocketPlugin>()).Where(p => p != null).ToList<IRocketPlugin>(); } }
        
        /// <summary>
        /// Maps assembly name to .dll file path.
        /// </summary>
        private Dictionary<AssemblyName, string> libraries = new Dictionary<AssemblyName, string>();

        public List<IRocketPlugin> GetPlugins()
        {
            return Plugins;
        }

        public IRocketPlugin GetPlugin(Assembly assembly)
        {
            return plugins.Select(g => g.GetComponent<IRocketPlugin>()).Where(p => p != null && p.GetType().Assembly == assembly).FirstOrDefault();
        }

        public IRocketPlugin GetPlugin(string name)
        {
            return plugins.Select(g => g.GetComponent<IRocketPlugin>()).Where(p => p != null && ((IRocketPlugin)p).Name == name).FirstOrDefault();
        }

        private Assembly OnAssemblyResolve(object sender, ResolveEventArgs args)
        {
            try
            {
                AssemblyName requestedName = new AssemblyName(args.Name);
                var matchesByName = libraries.Where(lib => string.Equals(lib.Key.Name, requestedName.Name));
                
                // Prefer exactly-matching version if possible.
                var bestMatch = matchesByName.FirstOrDefault(lib => lib.Key.Version == requestedName.Version);
                if (string.IsNullOrEmpty(bestMatch.Value))
                {
                    // Otherwise, fallback to highest version.
                    bestMatch = matchesByName.OrderByDescending(lib => lib.Key.Version).FirstOrDefault();
                }
                if (!string.IsNullOrEmpty(bestMatch.Value))
                {
                    // https://github.com/SmartlyDressedGames/Legally-Distinct-Missile/issues/84
                    if (requestedName.Version != null)
                    {
                        if (bestMatch.Key.Version == null)
                        {
                            Logging.Logger.LogWarning($"Rocket best match for dependency {requestedName} has no configued version: {bestMatch.Key} at {bestMatch.Value}");
                        }
                        else if (bestMatch.Key.Version < requestedName.Version)
                        {
                            Logging.Logger.LogWarning($"Rocket best match for dependency {requestedName} version is older than requested: {bestMatch.Key} at {bestMatch.Value}");
                        }
                    }

                    return Assembly.Load(File.ReadAllBytes(bestMatch.Value));
                }
            }
            catch (Exception ex)
            {
                Logging.Logger.LogException(ex, "Rocket caught exception resolving dependency: " + args.Name);
            }

            Logging.Logger.LogError("Rocket could not find dependency: " + args.Name);
            return null;
        }

        private void Awake() {
            AppDomain.CurrentDomain.AssemblyResolve += OnAssemblyResolve;
            SDG.Framework.Modules.ModuleHook.PreVanillaAssemblyResolvePostRedirects += OnAssemblyResolve;
        }

        private void Start()
        {
            loadPlugins();
        }

        public Type GetMainTypeFromAssembly(Assembly assembly)
        {
            return RocketHelper.GetTypesFromInterface(assembly, "IRocketPlugin").FirstOrDefault();
        }

        private void loadPlugins()
        {
            libraries = FindAssembliesInDirectory(Environment.LibrariesDirectory);
            foreach(KeyValuePair<AssemblyName,string> pair in FindAssembliesInDirectory(Environment.PluginsDirectory))
            {
                if(!libraries.ContainsKey(pair.Key))
                    libraries.Add(pair.Key,pair.Value);
            }

            foreach (KeyValuePair<AssemblyName, string> pair in libraries)
            {
                Logging.Logger.Log($"Rocket dependency registered: {pair.Key} at {pair.Value}");
            }

            pluginAssemblies = LoadAssembliesFromDirectory(Environment.PluginsDirectory);
            List<Type> pluginImplemenations = RocketHelper.GetTypesFromInterface(pluginAssemblies, "IRocketPlugin");
            foreach (Type pluginType in pluginImplemenations)
            {
                GameObject plugin = new GameObject(pluginType.Name, pluginType);
                DontDestroyOnLoad(plugin);
                plugins.Add(plugin);
            }
            OnPluginsLoaded.TryInvoke();
        }

        private void unloadPlugins() {
            for(int i = plugins.Count; i > 0; i--)
            {
                Destroy(plugins[i-1]);
            }
            plugins.Clear();
        }

        internal void Reload()
        {
            unloadPlugins();
            loadPlugins();
        }

        public static Dictionary<string, string> GetAssembliesFromDirectory(string directory, string extension = "*.dll")
        {
            Dictionary<string, string> l = new Dictionary<string, string>();
            IEnumerable<FileInfo> libraries = new DirectoryInfo(directory).GetFiles(extension, SearchOption.AllDirectories);
            foreach (FileInfo library in libraries)
            {
                try
                {
                    AssemblyName name = AssemblyName.GetAssemblyName(library.FullName);
                    l.Add(name.FullName, library.FullName);
                }
                catch { }
            }
            return l;
        }

        /// <summary>
        /// Replacement for GetAssembliesFromDirectory using AssemblyName as key rather than string.
        /// </summary>
        private static Dictionary<AssemblyName, string> FindAssembliesInDirectory(string directory)
        {
            Dictionary<AssemblyName, string> l = new Dictionary<AssemblyName, string>();
            IEnumerable<FileInfo> libraries = new DirectoryInfo(directory).GetFiles("*.dll", SearchOption.AllDirectories);
            foreach (FileInfo library in libraries)
            {
                try
                {
                    AssemblyName name = AssemblyName.GetAssemblyName(library.FullName);
                    l.Add(name, library.FullName);
                }
                catch { }
            }
            return l;
        }

        public static List<Assembly> LoadAssembliesFromDirectory(string directory, string extension = "*.dll")
        {
            List<Assembly> assemblies = new List<Assembly>();
            IEnumerable<FileInfo> pluginsLibraries = new DirectoryInfo(directory).GetFiles(extension, SearchOption.AllDirectories);

            foreach (FileInfo library in pluginsLibraries)
            {
                try
                {
                    Assembly assembly = Assembly.LoadFile(library.FullName);//Assembly.Load(File.ReadAllBytes(library.FullName));

                    List<Type> types = RocketHelper.GetTypesFromInterface(assembly, "IRocketPlugin").FindAll(x => !x.IsAbstract);

                    if (types.Count == 1)
                    {
                        Logging.Logger.Log("Loading "+ assembly.GetName().Name +" from "+ assembly.Location);
                        assemblies.Add(assembly);
                    }
                    else
                    {
                        Logging.Logger.LogError("Invalid or outdated plugin assembly: " + assembly.GetName().Name);
                    }
                }
                catch (Exception ex)
                {
                    Logging.Logger.LogError(ex, "Could not load plugin assembly: " + library.Name);
                }
            }
            return assemblies;
        }
    }
}