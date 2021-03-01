using Rocket.Unturned.Player;
using SDG.Unturned;
using UnityEngine;

namespace Rocket.Unturned.Effects
{
    public class UnturnedEffect
    {
        public UnturnedEffect(string type, ushort effectID, bool global)
        {
            this.Type = type;
            this.EffectID = effectID;
            this.Global = global;
        }
        public string Type;
        public ushort EffectID;
        public bool Global;

        public void Trigger(UnturnedPlayer player)
        {
            if (!Global)
            {
                TriggerEffectParameters parameters = new TriggerEffectParameters(EffectID);
                parameters.position = player.Player.transform.position;
                parameters.relevantPlayerID = player.CSteamID;
                EffectManager.triggerEffect(parameters);
            }
            else
            {
                TriggerEffectParameters parameters = new TriggerEffectParameters(EffectID);
                parameters.position = player.Player.transform.position;
                parameters.relevantDistance = 1024.0f;
                EffectManager.triggerEffect(parameters);
            }
        }

        public void Trigger(Vector3 position)
        {
            TriggerEffectParameters parameters = new TriggerEffectParameters(EffectID);
            parameters.position = position;
            parameters.relevantDistance = 1024.0f;
            EffectManager.triggerEffect(parameters);
        }
    }
}
