using SDG.Unturned;
using UnityEngine;
using Rocket.Unturned.Player;
using System.Collections.Generic;
using Rocket.API;
using Rocket.Unturned.Chat;

namespace Rocket.Unturned.Commands
{
    public class CommandTpwp : IRocketCommand
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
            get { return "tpwp"; }
        }

        public string Help
        {
            get { return "Teleports you to waypoint location";}
        }

        public string Syntax
        {
            get { return ""; }
        }

        public List<string> Aliases
        {
            get { return new List<string>(); }
        }

        public List<string> Permissions
        {
            get { return new List<string>() { "rocket.tpwp", "rocket.teleportwp" }; }
        }

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;

            //if (player.Stance == EPlayerStance.DRIVING || player.Stance == EPlayerStance.SITTING)
            //{
            //    UnturnedChat.Say(player, U.Translate("command_generic_teleport_while_driving_error"));
            //    throw new WrongUsageOfCommandException(caller, this);
            //}

            if (!player.Player.quests.isMarkerPlaced)
            {
                UnturnedChat.Say(player, U.Translate("command_tpwp_marker_not_set"));
                throw new WrongUsageOfCommandException(caller, this);
            }

            Vector3 position = player.Player.quests.markerPosition;
            if (!RaycastFromSkyToPosition(ref position))
            {
                UnturnedChat.Say(player, U.Translate("command_tpwp_failed_raycast"));
                throw new WrongUsageOfCommandException(caller, this);
            }

            player.Player.teleportToLocationUnsafe(new Vector3(position.x, position.y, position.z), player.Rotation);
            Core.Logging.Logger.Log(U.Translate("command_tp_teleport_console", player.CharacterName, position.x + "," + position.y + "," + position.z));
            UnturnedChat.Say(player, U.Translate("command_tp_teleport_private", position.x + "," + position.y + "," + position.z));
        }

        // Copy of Unturned function because its protected
        protected static bool RaycastFromSkyToPosition(ref Vector3 position)
        {
            position.y = 1024f;
            RaycastHit raycastHit;
            if (Physics.Raycast(position, Vector3.down, out raycastHit, 2048f, RayMasks.WAYPOINT))
            {
                position = raycastHit.point + Vector3.up;
                return true;
            }
            return false;
        }
    }
}