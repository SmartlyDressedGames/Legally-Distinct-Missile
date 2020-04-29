﻿using System;
using System.Collections.Generic;
using System.Linq;
using Rocket.API;
using Rocket.Core;
using Rocket.Core.Serialization;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;
using UnityEngine;

namespace Rocket.Unturned.Commands
{
    public sealed class UnturnedCommands : MonoBehaviour
    {
        private void Awake()
        {
            foreach(var vanillaCommand in Commander.commands)
            {
                R.Commands.Register(new UnturnedVanillaCommand(vanillaCommand),vanillaCommand.command.ToLower(),CommandPriority.Low);
            }
        }

        internal class UnturnedVanillaCommand : IRocketCommand
        {
            public Command command;

            public UnturnedVanillaCommand(Command command)
            {
                this.command = command;
            }

            public List<string> Aliases
            {
                get
                {
                   return new List<string>();
                }
            }

            public AllowedCaller AllowedCaller
            {
                get
                {
                    return AllowedCaller.Both;
                }
            }

            public string Help
            {
                get
                {
                    return command.help;
                }
            }

            public string Name
            {
                get
                {
                    return command.command;
                }
            }

            public List<string> Permissions
            {
                get
                {
                    return new List<string> { "unturned."+command.command.ToLower() };
                }
            }

            public string Syntax
            {
                get
                {
                    return command.info.Replace("/"," ");
                }
            }

            public void Execute(IRocketPlayer caller, string[] command)
            {
                var id = CSteamID.Nil;
                if(caller is UnturnedPlayer)
                {
                    id = ((UnturnedPlayer)caller).CSteamID;
                }
                Commander.commands.Where(c => c.command == Name).FirstOrDefault()?.check(id,Name, String.Join("/", command));
            }
        }




    }
}