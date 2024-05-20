using Rocket.Core.Assets;
using System;
using System.IO;
using Rocket.API;
using System.Xml.Serialization;
using SDG.Framework.IO.Serialization;
using Newtonsoft.Json;
using SDG.Framework.IO.Deserialization;

namespace Rocket.Core.Assets
{
    public class JsonFileAsset<T> : Asset<T> where T : class, IDefaultable
    {
        private JSONDeserializer deserializer;
        private string file;
        T defaultInstance;

        public JsonFileAsset(string file, Type[] extraTypes = null, T defaultInstance = null)
        {
            
            this.deserializer = new JSONDeserializer();
            this.file = file;
            this.defaultInstance = defaultInstance;
            Load();
        }

        public override T Save()
        {
            
            try
            {
                string directory = Path.GetDirectoryName(file);
                if (!String.IsNullOrEmpty(directory) && !Directory.Exists(directory)) Directory.CreateDirectory(directory);
                

                if (instance == null)
                {
                    if (defaultInstance == null)
                    {
                        instance = Activator.CreateInstance<T>();
                        instance.LoadDefaults();
                    }
                    else
                    {
                        instance = defaultInstance;
                    }
                }
                File.WriteAllText(file,JsonConvert.SerializeObject(instance, Formatting.Indented));
                return instance;
                
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("Failed to serialize JSONFileAsset: {0}", file), ex);
            }
        }

        public override void Load(AssetLoaded<T> callback = null)
        {
            try
            {
                if (!String.IsNullOrEmpty(file) && File.Exists(file))
                {
                    instance = deserializer.deserialize<T>(file);
                }

                Save();

                if (callback != null)
                    callback(this);
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("Failed to deserialize JSONFileAsset: {0}", file), ex);
            }
        }
        
    }
}
