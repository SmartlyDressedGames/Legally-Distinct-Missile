#region

using UnityEngine;

#endregion

namespace Rocket.Unturned.Player
{
    public class UnturnedPlayerComponent : MonoBehaviour
    {
        public UnturnedPlayer Player { get; private set; }


        protected virtual void Load()
        {
        }

        protected virtual void Unload()
        {
        }


        #region Start_Stop

        private void Awake()
        {
            Player = UnturnedPlayer.FromPlayer(gameObject.transform.GetComponent<SDG.Unturned.Player>());
        }

        private void OnEnable()
        {
            Load();
        }

        private void OnDisable()
        {
            Unload();
        }

        #endregion
    }
}