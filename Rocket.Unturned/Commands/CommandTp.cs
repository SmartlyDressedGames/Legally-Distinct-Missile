using SDG.Unturned;
using UnityEngine;
using System.Linq;
using Rocket.Core.Logging;
using Rocket.Unturned.Player;
using System.Collections.Generic;
using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.API.Extensions;

namespace Rocket.Unturned.Commands
{
    public class CommandTp : IRocketCommand
    {
        public AllowedCaller AllowedCaller
        {
            get
            {
                return AllowedCaller.Player;
            }
        }

        public string Name
        {
            get { return "tp"; }
        }

        public string Help
        {
            get { return "Teleports you to another player or location";}
        }

        public string Syntax
        {
            get { return "<player | place | x y z>"; }
        }

        public List<string> Aliases
        {
            get { return new List<string>(); }
        }

        public List<string> Permissions
        {
            get { return new List<string>() { "rocket.tp", "rocket.teleport" }; }
        }

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;
            if (command.Length != 1 && command.Length != 3)
            {
                UnturnedChat.Say(player, U.Translate("command_generic_invalid_parameter"));
                throw new WrongUsageOfCommandException(caller, this);
            }

            if (player.Stance == EPlayerStance.DRIVING || player.Stance == EPlayerStance.SITTING)
            {
                UnturnedChat.Say(player, U.Translate("command_generic_teleport_while_driving_error"));
                throw new WrongUsageOfCommandException(caller, this);
            }

            float? x = null;
            float? y = null;
            float? z = null;

            if (command.Length == 3)
            {
                //Trim comma's and parentheses out of the command, if using a copy/pasted location.
                command[0] = command[0].Trim(',', '(');
                command[1] = command[1].Trim(',');
                command[2] = command[2].Trim(',', ')');
                x = command.GetFloatParameter(0);
                y = command.GetFloatParameter(1);
                z = command.GetFloatParameter(2);
            }
            if (x != null && y != null && z != null)
            {
                Vector3 pos = new Vector3((float)x, (float)y, (float)z);
                if (!player.Teleport(pos, player.Rotation, caller.IsAdmin))
                {
                    UnturnedChat.Say(player, U.Translate("command_tp_failed_obstructed"));
                    return;
                }
                Core.Logging.Logger.Log(U.Translate("command_tp_teleport_console", player.CharacterName, pos.ToString().Trim('(', ')')));
                UnturnedChat.Say(player, U.Translate("command_tp_teleport_private", pos.ToString().Trim('(', ')')));
            }
            else
            {
                UnturnedPlayer otherplayer = UnturnedPlayer.FromName(command[0]);
                if (otherplayer != null && otherplayer != player)
                {
                    if (!player.Teleport(otherplayer, caller.IsAdmin))
                    {
                        UnturnedChat.Say(player, U.Translate("command_tp_failed_obstructed"));
                        return;
                    }
                    Core.Logging.Logger.Log(U.Translate("command_tp_teleport_console", player.CharacterName, otherplayer.CharacterName));
                    UnturnedChat.Say(player, U.Translate("command_tp_teleport_private", otherplayer.CharacterName));
                }
                else
                {
                    Node item = LevelNodes.nodes.Where(n => n.type == ENodeType.LOCATION && ((LocationNode)n).name.ToLower().Contains(command[0].ToLower())).FirstOrDefault();
                    if (item != null)
                    {
                        Vector3 c = item.point + new Vector3(0f, 0.5f, 0f);
                        if (!player.Teleport(c, player.Rotation, caller.IsAdmin))
                        {
                            UnturnedChat.Say(player, U.Translate("command_tp_failed_obstructed"));
                            return;
                        }
                        Core.Logging.Logger.Log(U.Translate("command_tp_teleport_console", player.CharacterName, ((LocationNode)item).name));
                        UnturnedChat.Say(player, U.Translate("command_tp_teleport_private", ((LocationNode)item).name));
                    }
                    else
                    {
                        UnturnedChat.Say(player, U.Translate("command_tp_failed_find_destination"));
                    }
                }
            }
        }
    }
}