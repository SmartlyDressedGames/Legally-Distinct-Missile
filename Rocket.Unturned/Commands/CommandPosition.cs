using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rocket.Unturned.Commands;
public class CommandPosition : IRocketCommand
{
    public AllowedCaller AllowedCaller => AllowedCaller.Player;

    public string Name => "position";

    public string Help => "Get current position of the player";

    public string Syntax => "";

    public List<string> Aliases => new List<string> { "pos" };

    public List<string> Permissions => new List<string> { "rocket.position", "rocket.pos" };

    public void Execute(IRocketPlayer caller, string[] command)
    {
        UnturnedPlayer player = (UnturnedPlayer)caller;

        UnturnedChat.Say(player, U.Translate("command_position_get", player.Position.ToString()));
    }
}
