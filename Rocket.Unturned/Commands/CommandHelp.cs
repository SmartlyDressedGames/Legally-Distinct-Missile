using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Rocket.API;
using Rocket.Core;
using SDG.Unturned;

namespace Rocket.Unturned.Commands
{
    public class CommandHelp : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Both;

        public string Name => "help";

        public string Help => "Shows you a specific help";

        public string Syntax => "[command]";

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string> { "rocket.help" };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            if (command.Length == 0)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("[Vanilla]");
                Console.ForegroundColor = ConsoleColor.White;
                Commander.commands.OrderBy(c => c.command).All(c => { Console.WriteLine(c.command.ToLower().PadRight(20, ' ') + " " + c.info.Replace(c.command, "").TrimStart().ToLower()); return true; });

                Console.WriteLine();

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("[Rocket]");
                Console.ForegroundColor = ConsoleColor.White;
                R.Commands.Commands.Where(c => c.GetType().Assembly == Assembly.GetExecutingAssembly()).OrderBy(c => c.Name).All(c => { Console.WriteLine(c.Name.ToLower().PadRight(20, ' ') + " " + c.Syntax.ToLower()); return true; });

                Console.WriteLine();
                
                foreach (IRocketPlugin plugin in R.Plugins.GetPlugins())
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("[" + plugin.GetType().Assembly.GetName().Name + "]");
                    Console.ForegroundColor = ConsoleColor.White;
                    R.Commands.Commands.Where(c => c.GetType().Assembly == plugin.GetType().Assembly).OrderBy(c => c.Name).All(c => { Console.WriteLine(c.Name.ToLower().PadRight(20, ' ') + " " + c.Syntax.ToLower()); return true; });
                    Console.WriteLine();
                }
            }
            else
            {
                IRocketCommand cmd = R.Commands.Commands.FirstOrDefault(c => (string.Compare(c.Name, command[0], StringComparison.OrdinalIgnoreCase) == 0));
                if (cmd != null)
                {
                    string commandName = cmd.GetType().Assembly.GetName().Name + " / " + cmd.Name;
                   
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("[" + commandName + "]");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine(cmd.Name + "\t\t" + cmd.Syntax);
                    Console.WriteLine(cmd.Help);
                }
            }
        }
    }
}