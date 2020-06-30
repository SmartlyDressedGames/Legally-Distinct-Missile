using Rocket.Core.Logging;
using SDG.Unturned;
using System;
using UnityEngine;

namespace Rocket.Unturned.Utils
{
    internal class AutomaticSaveWatchdog : MonoBehaviour
    {
        private void Update()
        {
            CheckTimer();
        }

        private DateTime? _nextSaveTime = null;
        private static AutomaticSaveWatchdog _instance;
        private int _interval = 30;

        private void Start()
        {
            _instance = this;
            if (U.Settings.Instance.AutomaticSave.Enabled)
            {
                if(U.Settings.Instance.AutomaticSave.Interval < _interval)
                {
                    Core.Logging.Logger.LogError("AutomaticSave interval must be atleast 30 seconds, changed to 30 seconds");
                }
                else
                {
                    _interval = U.Settings.Instance.AutomaticSave.Interval;
                }
                Core.Logging.Logger.Log($"This server will automatically save every {_interval} seconds");
                RestartTimer();
            }
        }

        private void RestartTimer ()
        {
            _nextSaveTime = DateTime.Now.AddSeconds(_interval);
        }

        private void CheckTimer()
        {
            try
            {
                if (_nextSaveTime == null || _nextSaveTime.Value >= DateTime.Now) return;
                Core.Logging.Logger.Log("Saving server");
                RestartTimer();
                SaveManager.save();
            }
            catch (Exception er)
            {
                Core.Logging.Logger.LogException(er);
            }
        }
    }
}
