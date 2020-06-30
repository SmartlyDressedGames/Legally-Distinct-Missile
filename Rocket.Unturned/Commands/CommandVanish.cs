using Rocket.API;
using Rocket.Core.Logging;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using System.Collections.Generic;

namespace Rocket.Unturned.Commands
{
    public class CommandVanish : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "vanish";

        public string Help => "Are we rushing in or are we goin' sneaky beaky like?";

        public string Syntax => "";

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string>() { "rocket.vanish" };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;
            if (player.Features.VanishMode)
            {
                Logger.Log(U.Translate("command_vanish_disable_console", player.CharacterName));
                UnturnedChat.Say(caller, U.Translate("command_vanish_disable_private"));
                player.Features.VanishMode = false;
            }
            else
            {
                Logger.Log(U.Translate("command_vanish_enable_console", player.CharacterName));
                UnturnedChat.Say(caller, U.Translate("command_vanish_enable_private"));
                player.Features.VanishMode = true;
            }
        }
    }
}