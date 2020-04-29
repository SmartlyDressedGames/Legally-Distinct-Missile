using Rocket.Unturned.Player;
using SDG.Unturned;

namespace Rocket.Unturned.Extensions
{
    public static class SteamPlayerExtension
    {
        public static UnturnedPlayer ToUnturnedPlayer(this SteamPlayer player)
        {
            return UnturnedPlayer.FromSteamPlayer(player);
        }
    }
}