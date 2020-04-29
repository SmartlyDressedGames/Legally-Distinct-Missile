using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Rocket.API;
using Rocket.Core.Utils;
using Rocket.Unturned.Player;
using UnityEngine;
using Logger = Rocket.Core.Logging.Logger;

namespace Rocket.Unturned.Plugins
{
    public sealed class PluginUnturnedPlayerComponentManager : MonoBehaviour
    {
        private Assembly _assembly;
        private List<Type> _unturnedPlayerComponents = new List<Type>();

        private void OnEnable()
        {
            try
            {
                _assembly = GetComponent<IRocketPlugin>().GetType().Assembly;

                U.Events.OnBeforePlayerConnected += AddPlayerComponents;
                _unturnedPlayerComponents.AddRange(RocketHelper.GetTypesFromParentClass(_assembly, typeof(UnturnedPlayerComponent)));

                foreach (var playerComponent in _unturnedPlayerComponents)
                {
                    Logger.Log("Adding UnturnedPlayerComponent: " + playerComponent.Name);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }
        
        private void OnDisable()
        {
            try
            {
                U.Events.OnPlayerConnected -= AddPlayerComponents;
                _unturnedPlayerComponents = _unturnedPlayerComponents.Where(p => p.Assembly != _assembly).ToList();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        private void AddPlayerComponents(IRocketPlayer rocketPlayer)
        {
            var player = (UnturnedPlayer)rocketPlayer;

            foreach (var component in _unturnedPlayerComponents)
            {
                player.Player.gameObject.AddComponent(component);
            }
        }
    }
}