using System;
using Rocket.Unturned.Player;
using SDG.Unturned;
using UnityEngine;
using Logger = Rocket.Core.Logging.Logger;

namespace Rocket.Unturned
{
    public class UnturnedPlayerMovement : UnturnedPlayerComponent
    {
        public bool VanishMode = false;

        private DateTime _lastUpdate = DateTime.Now;
        private Vector3 _lastVector = new Vector3(0,-1,0);

        private void FixedUpdate()
        {
            var movement = Player.GetComponent<PlayerMovement>();

            if (!VanishMode)
            {
                if (U.Settings.Instance.LogSuspiciousPlayerMovement && _lastUpdate.AddSeconds(1) < DateTime.Now)
                {
                    _lastUpdate = DateTime.Now;

                    var positon = movement.real;

                    if (_lastVector.y != -1)
                    {
                        var y = positon.y - _lastVector.y;

                        if (y > 15)
                        {
                            Physics.Raycast(positon, Vector3.down, out var raycastHit);

                            var floor = raycastHit.point;
                            var distance = Math.Abs(floor.y - positon.y);
                            Logger.Log(Player.DisplayName + " moved x:" + positon.x + " y:" + positon.y + "(+" + y + ") z:" + positon.z + " in the last second (" + distance + ")");
                        }
                    }

                    _lastVector = movement.real;
                }
            }
        }
    }
}