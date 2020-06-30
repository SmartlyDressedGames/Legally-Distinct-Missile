using Rocket.Core;
using Rocket.Core.Logging;
using Rocket.Core.Utils;
using Rocket.Unturned.Player;
using SDG.Provider;
using SDG.Provider.Services.Achievements;
using SDG.Unturned;
using Steamworks;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rocket.Unturned
{
    public class UnturnedPlayerMovement : UnturnedPlayerComponent
    {
        public bool VanishMode = false;
        private DateTime _lastUpdate = DateTime.Now;
        private Vector3 _lastVector = new Vector3(0,-1,0);

        private void FixedUpdate()
        {
            PlayerMovement movement = (PlayerMovement)Player.GetComponent<PlayerMovement>();

            if (!VanishMode)
            {
                if (U.Settings.Instance.LogSuspiciousPlayerMovement && _lastUpdate.AddSeconds(1) < DateTime.Now)
                {
                    _lastUpdate = DateTime.Now;

                    Vector3 positon = movement.real;

                    if (_lastVector.y != -1)
                    {
                        float x = Math.Abs(_lastVector.x - positon.x);
                        float y = positon.y - _lastVector.y;
                        float z = Math.Abs(_lastVector.z - positon.z);
                        if (y > 15)
                        {
                            Physics.Raycast(positon, Vector3.down, out RaycastHit raycastHit);
                            Vector3 floor = raycastHit.point;
                            float distance = Math.Abs(floor.y - positon.y);
                            Core.Logging.Logger.Log(Player.DisplayName + " moved x:" + positon.x + " y:" + positon.y + "(+" + y + ") z:" + positon.z + " in the last second (" + distance + ")");
                        }
                    }
                    _lastVector = movement.real;
                }
            }
        }
    }
}
