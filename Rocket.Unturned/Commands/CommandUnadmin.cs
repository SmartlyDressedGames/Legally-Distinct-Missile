using Rocket.API;
using Rocket.Core;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using System.Collections.Generic;

namespace Rocket.Unturned.Commands
{
    public class CommandUnadmin : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Both;

        public string Name => "unadmin";

        public string Help => "Revoke a players admin privileges";

        public string Syntax => "";

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string>() { "rocket.unadmin" };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            if (R.Settings.Instance.WebPermissions.Enabled) return;
            UnturnedPlayer player = command.GetUnturnedPlayerParameter(0);
            if (player == null)
            {
                UnturnedChat.Say(caller, U.Translate("command_generic_invalid_parameter"));
                throw new WrongUsageOfCommandException(caller, this);
            }

            if (!player.IsAdmin) return;
            
            UnturnedChat.Say(caller, "Successfully unadmined " + player.CharacterName);
            player.Admin(false);
        }
    }
}