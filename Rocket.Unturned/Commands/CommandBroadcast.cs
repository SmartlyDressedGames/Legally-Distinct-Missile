﻿using System.Collections.Generic;
using Rocket.API;
using Rocket.API.Extensions;
using Rocket.Unturned.Chat;
using UnityEngine;

namespace Rocket.Unturned.Commands
{
    public class CommandBroadcast : IRocketCommand
    {
        public AllowedCaller AllowedCaller
        {
            get
            {
                return AllowedCaller.Both;
            }
        }

        public string Name
        {
            get { return "broadcast"; }
        }

        public string Help
        {
            get { return "Broadcast a message"; }
        }

        public string Syntax
        {
            get { return "<color> <message>"; }
        }

        public List<string> Aliases
        {
            get { return new List<string>(); }
        }

        public List<string> Permissions
        {
            get
            {
                return new List<string> { "rocket.broadcast" };
            }
        }

        public void Execute(IRocketPlayer caller, string[] command)
        {
            var color = command.GetColorParameter(0);

            var i = 1;
            if (color == null) i = 0;
            var message = command.GetParameterString(i);

            if (message == null)
            {
                UnturnedChat.Say(caller, U.Translate("command_generic_invalid_parameter"));
                throw new WrongUsageOfCommandException(caller, this);
            }

            UnturnedChat.Say(message, color.HasValue ? (Color)color : Color.green);
        }
    }
}