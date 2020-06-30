using Rocket.API;
using Rocket.API.Extensions;
using Rocket.Core.Logging;
using Rocket.Core.Utils;
using Rocket.Unturned.Events;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Rocket.Unturned.Plugins
{
    public sealed class PluginUnturnedPlayerComponentManager : MonoBehaviour
    {
        private Assembly _assembly;
        private List<Type> _unturnedPlayerComponents = new List<Type>();
        
        private void OnDisable()
        {
            try
            {
                U.Events.OnPlayerConnected -= AddPlayerComponents;
                _unturnedPlayerComponents = _unturnedPlayerComponents.Where(p => p.Assembly != _assembly).ToList();
                List<Type> playerComponents = RocketHelper.GetTypesFromParentClass(_assembly, typeof(UnturnedPlayerComponent));
                foreach (Type playerComponent in playerComponents)
                {
                    //Provider.Players.ForEach(p => p.Player.gameObject.TryRemoveComponent(playerComponent.GetType()));
                }
            }
            catch (Exception ex)
            {
                Core.Logging.Logger.LogException(ex);
            }
        }

        private void OnEnable()
        {
            try
            {  
                IRocketPlugin plugin = GetComponent<IRocketPlugin>();
                _assembly = plugin.GetType().Assembly;

                U.Events.OnBeforePlayerConnected += AddPlayerComponents;
                _unturnedPlayerComponents.AddRange(RocketHelper.GetTypesFromParentClass(_assembly, typeof(UnturnedPlayerComponent)));

                foreach (Type playerComponent in _unturnedPlayerComponents)
                {
                    Core.Logging.Logger.Log("Adding UnturnedPlayerComponent: "+playerComponent.Name);
                    //Provider.Players.ForEach(p => p.Player.gameObject.TryAddComponent(playerComponent.GetType()));
                }
            }
            catch (Exception ex)
            {
                Core.Logging.Logger.LogException(ex);
            }
        }

        private void AddPlayerComponents(IRocketPlayer p)
        {
            foreach (Type component in _unturnedPlayerComponents)
            {
                ((UnturnedPlayer)p).Player.gameObject.AddComponent(component);
            }
        }
    }
}