#region

using System;
using System.Collections;
using SDG.Unturned;
using UnityEngine;
using Logger = Rocket.Core.Logging.Logger;

#endregion


namespace Rocket.Unturned.Utils
{
    internal class AutomaticSaveWatchdog : MonoBehaviour
    {
        private const uint MinInterval = 30;


        public static AutomaticSaveWatchdog Instance { get; private set; }
        public Coroutine AutoSaveCoroutine { get; private set; }


        private uint _interval = MinInterval;
        public uint Interval
        {
            get => _interval;
            set
            {
                StopAutoSave();
                _interval = CheckInterval(value) ? value : MinInterval;
                StartAutoSave();
            }
        }


        #region Coroutines

        private IEnumerator AutoSave()
        {
            var duration = Interval * 60;
            {
                yield return new WaitForSeconds(duration);


                try
                {
                    Logger.Log("Saving server");
                    SaveManager.save();
                }
                catch (Exception ex)
                {
                    Logger.LogException(ex, "Error during auto-save");
                }
            }
            while (true) ;
        }

        #endregion


        #region Start_Stop

        private void Start()
        {
            Instance = this;
            if (!U.Settings.Instance.AutomaticSave.Enabled)
                return;


            var value = U.Settings.Instance.AutomaticSave.Interval;
            _interval = CheckInterval(value) ? value : MinInterval;
            Logger.Log($"This server will automatically save every {Interval} seconds");
            StartAutoSave();
        }

        private void OnDisable()
        {
            StopAutoSave();
            Instance = null;
        }

        #endregion


        #region Functions

        private static bool CheckInterval(uint value)
        {
            if (value >= MinInterval)
                return true;


            Logger.LogError("AutomaticSave interval must be at least 30 seconds, changed to 30 seconds");
            return false;
        }

        private void StartAutoSave()
        {
            AutoSaveCoroutine = StartCoroutine(AutoSave());
            if (AutoSaveCoroutine != null)
                StopCoroutine(AutoSaveCoroutine);
        }

        private void StopAutoSave()
        {
            if (AutoSaveCoroutine != null)
                StopCoroutine(AutoSaveCoroutine);
        }

        #endregion
    }
}