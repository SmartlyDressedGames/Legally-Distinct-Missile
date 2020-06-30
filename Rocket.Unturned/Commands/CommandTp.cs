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
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "tp";

        public string Help => "Teleports you to another player or location";

        public string Syntax => "<player | place | x y z>";

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string>() { "rocket.tp", "rocket.teleport" };

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
                x = command.GetFloatParameter(0);
                y = command.GetFloatParameter(1);
                z = command.GetFloatParameter(2);
            }
            if (x != null && y != null && z != null)
            {
                player.Teleport(new Vector3((float)x, (float)y, (float)z), MeasurementTool.angleToByte(player.Rotation));
                Core.Logging.Logger.Log(U.Translate("command_tp_teleport_console", player.CharacterName, (float)x + "," + (float)y + "," + (float)z));
                UnturnedChat.Say(player, U.Translate("command_tp_teleport_private", (float)x + "," + (float)y + "," + (float)z));
            }
            else
            {
                UnturnedPlayer otherPlayer = UnturnedPlayer.FromName(command[0]);
                if (otherPlayer != null && otherPlayer != player)
                {
                    player.Teleport(otherPlayer);
                    Core.Logging.Logger.Log(U.Translate("command_tp_teleport_console", player.CharacterName, otherPlayer.CharacterName));
                    UnturnedChat.Say(player, U.Translate("command_tp_teleport_private", otherPlayer.CharacterName));
                }
                else
                {
                    Node item = LevelNodes.nodes.FirstOrDefault(n => n.type == ENodeType.LOCATION && ((LocationNode)n).name.ToLower().Contains(command[0].ToLower()));
                    if (item != null)
                    {
                        Vector3 c = item.point + new Vector3(0f, 0.5f, 0f);
                        player.Teleport(c, MeasurementTool.angleToByte(player.Rotation));
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