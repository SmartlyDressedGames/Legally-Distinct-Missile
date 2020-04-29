using System;
using System.Diagnostics;
using System.IO;

namespace Rocket.Unturned.Launcher
{
    internal class RocketLauncher
    {
        private static TextReader _consoleReader;

        public static void Main(string[] args)
        {
            var instanceName = args.Length > 0 ? args[0] : "Rocket";

            var executableName = "";
            foreach (var fileName in new[]{ "Unturned_Headless.x86" , "Unturned.x86" , "Unturned.exe", "Unturned_Headless.x86_64", "Unturned.x86_64"})
            {
                if (File.Exists(fileName))
                {
                    executableName = fileName;
                    break;
                }
            }

            if (string.IsNullOrEmpty(executableName))
            {
                throw new FileNotFoundException("Could not locate Unturned executable.");
            }

            var consoleOutput = instanceName + ".console";

            var fileSystemWatcher = new FileSystemWatcher(".", consoleOutput);
            fileSystemWatcher.Changed += FileSystemWatcher_Changed;
            fileSystemWatcher.EnableRaisingEvents = true;

            _consoleReader = new StreamReader(new FileStream(consoleOutput, FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite));

            var process = new Process
            {
                StartInfo =
                {
                    FileName = executableName,
                    Arguments = $"-nographics -batchmode -logfile 'Servers/{instanceName}/unturned.log' +secureserver/{instanceName}",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardInput = false
                }
            };

            process.Start();
            process.WaitForExit();
        }

        private static void FileSystemWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType == WatcherChangeTypes.Changed)
            {
                var input = _consoleReader.ReadToEnd();
                if (!string.IsNullOrEmpty(input))
                {
                    Console.Write(input);
                }
            }
        }
    }
}