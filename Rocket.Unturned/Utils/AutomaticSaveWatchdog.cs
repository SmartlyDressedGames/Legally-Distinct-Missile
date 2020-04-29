using System;
using SDG.Unturned;
using UnityEngine;
using Logger = Rocket.Core.Logging.Logger;

namespace Rocket.Unturned.Utils
{
    internal class AutomaticSaveWatchdog : MonoBehaviour
    {
        public static AutomaticSaveWatchdog Instance;
        
        private int _interval = 30;
        private DateTime? _nextSaveTime;

        private void FixedUpdate()
        {
            CheckTimer();
        }
        
        private void Start()
        {
            Instance = this;

            if (U.Settings.Instance.AutomaticSave.Enabled)
            {
                if(U.Settings.Instance.AutomaticSave.Interval < _interval)
                {
                    Logger.LogError("AutomaticSave interval must be atleast 30 seconds, changed to 30 seconds.");
                }
                else
                {
                    _interval = U.Settings.Instance.AutomaticSave.Interval;
                }

                Logger.Log($"This server will automatically save every {_interval} seconds.");

                RestartTimer();
            }
        }

        private void RestartTimer()
        {
            _nextSaveTime = DateTime.Now.AddSeconds(_interval);
        }

        private void CheckTimer()
        {
            try
            {
                if (_nextSaveTime != null && _nextSaveTime.Value < DateTime.Now)
                {
                    Logger.Log("Saving server");
                    RestartTimer();
                    SaveManager.save();
                }
            }
            catch (Exception er)
            {
                Logger.LogException(er);
            }
        }
    }
}