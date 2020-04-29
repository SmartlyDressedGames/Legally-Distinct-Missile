#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Rocket.API;
using Rocket.Core.Utils;
using Rocket.Unturned.Player;
using SDG.Unturned;
using UnityEngine;
using Logger = Rocket.Core.Logging.Logger;

#endregion

namespace Rocket.Unturned.Plugins
{
    public sealed class PluginUnturnedPlayerComponentManager : MonoBehaviour
    {
        private readonly List<Type> _unturnedPlayerComponents = new List<Type>();
        private Assembly _assembly;


        #region Events

        private void OnConnect(SteamPlayer steamPlayer)
        {
            GiveAllComponents(steamPlayer.player.gameObject);
        }

        #endregion


        #region Functions

        private void GiveAllComponents(GameObject playerGameObject)
        {
            foreach (var playerComponent in _unturnedPlayerComponents.Where(comp =>
                playerGameObject.GetComponent(comp) == null)) playerGameObject.AddComponent(playerComponent);
        }

        #endregion


        #region Start_Stop

        private void OnEnable()
        {
            try
            {
                var plugin = GetComponent<IRocketPlugin>();
                _assembly = plugin.GetType().Assembly;


                //This is called before 'OnBeforePlayerConnected'
                _unturnedPlayerComponents.AddRange(
                    RocketHelper.GetTypesFromParentClass(_assembly, typeof(UnturnedPlayerComponent)));
                Provider.onEnemyConnected += OnConnect;


                //Apply to existing players
                foreach (var client in Provider.clients)
                    GiveAllComponents(client.player.gameObject);
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
                Provider.onEnemyConnected -= OnConnect;


                //Remove from existing players
                foreach (var comp in Provider.clients.SelectMany(client =>
                    _unturnedPlayerComponents
                        .Select(playerComponent => client.player.gameObject.GetComponent(playerComponent))
                        .Where(comp => comp != null))) Destroy(comp);


                _unturnedPlayerComponents.Clear();
                _assembly = null;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        #endregion
    }
}