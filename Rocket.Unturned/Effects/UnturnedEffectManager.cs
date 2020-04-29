using Rocket.Unturned.Player;
using SDG.Unturned;
using UnityEngine;

namespace Rocket.Unturned.Effects
{
    public class UnturnedEffect
    {
        public string Type;
        public ushort EffectID;
        public bool Global;

        public UnturnedEffect(string type, ushort effectId, bool global)
        {
            Type = type;
            EffectID = effectId;
            Global = global;
        }

        public void Trigger(UnturnedPlayer player)
        {
            if (!Global)
            {
                EffectManager.instance.channel.send("tellEffectPoint", player.CSteamID, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, EffectID, player.Player.transform.position);
            }
            else
            {
                EffectManager.instance.channel.send("tellEffectPoint", ESteamCall.CLIENTS, player.Player.transform.position, 1024, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, EffectID, player.Player.transform.position);
            }
        }

        public void Trigger(Vector3 position)
        {
            EffectManager.instance.channel.send("tellEffectPoint", ESteamCall.CLIENTS, position, 1024, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, EffectID, position);
        }
    }
}