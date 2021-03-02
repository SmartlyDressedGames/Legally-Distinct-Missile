using Rocket.Core.Logging;
using Rocket.Unturned.Enumerations;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;
using System;
using UnityEngine;
using System.Linq;
using Rocket.Core.Extensions;

namespace Rocket.Unturned.Events
{
    public sealed class UnturnedPlayerEvents : UnturnedPlayerComponent
    {
        protected override void Load()
        {
            Player.Player.life.onStaminaUpdated += onUpdateStamina;
            Player.Player.inventory.onInventoryAdded += onInventoryAdded;
            Player.Player.inventory.onInventoryRemoved += onInventoryRemoved;
            Player.Player.inventory.onInventoryResized += onInventoryResized;
            Player.Player.inventory.onInventoryUpdated += onInventoryUpdated;
        }

        private void Start()
        {
            UnturnedEvents.triggerOnPlayerConnected(Player);
        }

        internal static void TriggerReceive(SteamChannel instance, CSteamID d, byte[] a, int b,int size)
        {
#if DEBUG
            /*ESteamPacket eSteamPacket = (ESteamPacket)a[0];
            int num = a[1];

            if (eSteamPacket != ESteamPacket.UPDATE_VOICE && eSteamPacket != ESteamPacket.UPDATE_UDP_CHUNK && eSteamPacket != ESteamPacket.UPDATE_TCP_CHUNK)
            {
                object[] objects = SteamPacker.getObjects(d, 2, a, instance.Methods[num].Types);

                string o = "";
                foreach (object r in objects)
                {
                    o += r.ToString() + ",";
                }
                Logger.Log("Receive+" + d.ToString() + ": " + o + " - " + b);
            }*/
#endif
            return;
        }
        
        internal static void InternalOnPlayerStatIncremented(SDG.Unturned.Player player, EPlayerStat gameStat)
        {
            UnturnedPlayerEvents instance = player.GetComponent<UnturnedPlayerEvents>();
            UnturnedPlayer rp = UnturnedPlayer.FromPlayer(player);
            OnPlayerUpdateStat.TryInvoke(rp, gameStat);
            instance.OnUpdateStat.TryInvoke(rp, gameStat);
        }

        internal static void InternalOnShirtChanged(PlayerClothing clothing)
        {
            UnturnedPlayer rp = UnturnedPlayer.FromPlayer(clothing.player);
            OnPlayerWear.TryInvoke(rp, Wearables.Shirt, clothing.shirt, clothing.shirtQuality);
        }

        internal static void InternalOnPantsChanged(PlayerClothing clothing)
        {
            UnturnedPlayer rp = UnturnedPlayer.FromPlayer(clothing.player);
            OnPlayerWear.TryInvoke(rp, Wearables.Pants, clothing.pants, clothing.pantsQuality);
        }

        internal static void InternalOnHatChanged(PlayerClothing clothing)
        {
            UnturnedPlayer rp = UnturnedPlayer.FromPlayer(clothing.player);
            OnPlayerWear.TryInvoke(rp, Wearables.Hat, clothing.hat, clothing.hatQuality);
        }

        internal static void InternalOnBackpackChanged(PlayerClothing clothing)
        {
            UnturnedPlayer rp = UnturnedPlayer.FromPlayer(clothing.player);
            OnPlayerWear.TryInvoke(rp, Wearables.Backpack, clothing.backpack, clothing.backpackQuality);
        }

        internal static void InternalOnVestChanged(PlayerClothing clothing)
        {
            UnturnedPlayer rp = UnturnedPlayer.FromPlayer(clothing.player);
            OnPlayerWear.TryInvoke(rp, Wearables.Vest, clothing.vest, clothing.vestQuality);
        }

        internal static void InternalOnMaskChanged(PlayerClothing clothing)
        {
            UnturnedPlayer rp = UnturnedPlayer.FromPlayer(clothing.player);
            OnPlayerWear.TryInvoke(rp, Wearables.Mask, clothing.mask, clothing.maskQuality);
        }

        internal static void InternalOnGlassesChanged(PlayerClothing clothing)
        {
            UnturnedPlayer rp = UnturnedPlayer.FromPlayer(clothing.player);
            OnPlayerWear.TryInvoke(rp, Wearables.Mask, clothing.glasses, clothing.glassesQuality);
        }

        internal static void InternalOnGestureChanged(PlayerAnimator animator)
        {
            PlayerGesture rocketGesture;
            switch(animator.gesture)
            {
                case EPlayerGesture.NONE:
                    rocketGesture = PlayerGesture.None;
                    break;
                case EPlayerGesture.INVENTORY_START:
                    rocketGesture = PlayerGesture.InventoryOpen;
                    break;
                case EPlayerGesture.INVENTORY_STOP:
                    rocketGesture = PlayerGesture.InventoryClose;
                    break;
                case EPlayerGesture.PICKUP:
                    rocketGesture = PlayerGesture.Pickup;
                    break;
                case EPlayerGesture.PUNCH_LEFT:
                    rocketGesture = PlayerGesture.PunchLeft;
                    break;
                case EPlayerGesture.PUNCH_RIGHT:
                    rocketGesture = PlayerGesture.PunchRight;
                    break;
                case EPlayerGesture.SURRENDER_START:
                    rocketGesture = PlayerGesture.SurrenderStart;
                    break;
                case EPlayerGesture.SURRENDER_STOP:
                    rocketGesture = PlayerGesture.SurrenderStop;
                    break;
                case EPlayerGesture.POINT:
                    rocketGesture = PlayerGesture.Point;
                    break;
                case EPlayerGesture.WAVE:
                    rocketGesture = PlayerGesture.Wave;
                    break;
                case EPlayerGesture.SALUTE:
                    rocketGesture = PlayerGesture.Salute;
                    break;
                case EPlayerGesture.ARREST_START:
                    rocketGesture = PlayerGesture.Arrest_Start;
                    break;
                case EPlayerGesture.ARREST_STOP:
                    rocketGesture = PlayerGesture.Arrest_Stop;
                    break;
                case EPlayerGesture.REST_START:
                    rocketGesture = PlayerGesture.Rest_Start;
                    break;
                case EPlayerGesture.REST_STOP:
                    rocketGesture = PlayerGesture.Rest_Stop;
                    break;
                case EPlayerGesture.FACEPALM:
                    rocketGesture = PlayerGesture.Facepalm;
                    break;

                default:
                    // Rocket does not have an equivalent. Plugins should use the game's event instead because this
                    // listener only exists for backwards compatibility.
                    return;
            }

            UnturnedPlayerEvents instance = animator.GetComponent<UnturnedPlayerEvents>();
            UnturnedPlayer rp = UnturnedPlayer.FromPlayer(animator.player);
            OnPlayerUpdateGesture.TryInvoke(rp, rocketGesture);
            instance.OnUpdateGesture.TryInvoke(rp, rocketGesture);
        }
        
        internal static void InternalOnTellHealth(PlayerLife life)
        {
            UnturnedPlayerEvents instance = life.GetComponent<UnturnedPlayerEvents>();
            UnturnedPlayer rp = UnturnedPlayer.FromPlayer(life.player);
            OnPlayerUpdateHealth.TryInvoke(rp, life.health);
            instance.OnUpdateHealth.TryInvoke(rp, life.health);
        }

        internal static void InternalOnTellFood(PlayerLife life)
        {
            UnturnedPlayerEvents instance = life.GetComponent<UnturnedPlayerEvents>();
            UnturnedPlayer rp = UnturnedPlayer.FromPlayer(life.player);
            OnPlayerUpdateFood.TryInvoke(rp, life.food);
            instance.OnUpdateFood.TryInvoke(rp, life.food);
        }

        internal static void InternalOnTellWater(PlayerLife life)
        {
            UnturnedPlayerEvents instance = life.GetComponent<UnturnedPlayerEvents>();
            UnturnedPlayer rp = UnturnedPlayer.FromPlayer(life.player);
            OnPlayerUpdateWater.TryInvoke(rp, life.water);
            instance.OnUpdateWater.TryInvoke(rp, life.water);
        }

        internal static void InternalOnTellVirus(PlayerLife life)
        {
            UnturnedPlayerEvents instance = life.GetComponent<UnturnedPlayerEvents>();
            UnturnedPlayer rp = UnturnedPlayer.FromPlayer(life.player);
            OnPlayerUpdateVirus.TryInvoke(rp, life.virus);
            instance.OnUpdateVirus.TryInvoke(rp, life.virus);
        }

        internal static void InternalOnTellBleeding(PlayerLife life)
        {
            UnturnedPlayerEvents instance = life.GetComponent<UnturnedPlayerEvents>();
            UnturnedPlayer rp = UnturnedPlayer.FromPlayer(life.player);
            OnPlayerUpdateBleeding.TryInvoke(rp, life.isBleeding);
            instance.OnUpdateBleeding.TryInvoke(rp, life.isBleeding);
        }

        internal static void InternalOnExperienceChanged(PlayerSkills skills, uint oldExerience)
        {
            UnturnedPlayerEvents instance = skills.GetComponent<UnturnedPlayerEvents>();
            UnturnedPlayer rp = UnturnedPlayer.FromPlayer(skills.player);
            OnPlayerUpdateExperience.TryInvoke(rp, skills.experience);
            instance.OnUpdateExperience.TryInvoke(rp, skills.experience);
        }

        internal static void InternalOnTellBroken(PlayerLife life)
        {
            UnturnedPlayerEvents instance = life.GetComponent<UnturnedPlayerEvents>();
            UnturnedPlayer rp = UnturnedPlayer.FromPlayer(life.player);
            OnPlayerUpdateBroken.TryInvoke(rp, life.isBroken);
            instance.OnUpdateBroken.TryInvoke(rp, life.isBroken);
        }

        internal static void InternalOnRevived(PlayerLife sender)
        {
            UnturnedPlayerEvents instance = sender.GetComponent<UnturnedPlayerEvents>();
            UnturnedPlayer rp = UnturnedPlayer.FromPlayer(sender.player);

            // First parameter of tellLife was health.
            OnPlayerUpdateLife.TryInvoke(rp, sender.health);
            instance.OnUpdateLife.TryInvoke(rp, sender.health);

            // Ideally avoid using this angleToByte method in new code. Please.
            Vector3 position = sender.transform.position;
            byte angle = MeasurementTool.angleToByte(sender.transform.rotation.eulerAngles.y);
            OnPlayerRevive.TryInvoke(rp, position, angle);
            instance.OnRevive.TryInvoke(rp, position, angle);
        }

        internal static void InternalOnPlayerDied(PlayerLife sender, EDeathCause cause, ELimb limb, CSteamID instigator)
        {
            UnturnedPlayerEvents instance = sender.GetComponent<UnturnedPlayerEvents>();
            UnturnedPlayer rp = UnturnedPlayer.FromPlayer(sender.player);

            // First parameter of tellDead was the "ragdoll" force prior to client adjustments. Unnecessary AFAIK.
            OnPlayerDead.TryInvoke(rp, Vector3.zero);
            instance.OnDead.TryInvoke(rp, Vector3.zero);

            OnPlayerDeath.TryInvoke(rp, cause, limb, instigator);
            instance.OnDeath.TryInvoke(rp, cause, limb, instigator);
        }

        internal static void InternalOnStanceChanged(PlayerStance stance)
        {
            UnturnedPlayerEvents instance = stance.GetComponent<UnturnedPlayerEvents>();
            UnturnedPlayer rp = UnturnedPlayer.FromPlayer(stance.player);
            OnPlayerUpdateStance.TryInvoke(rp, (byte) stance.stance);
            instance.OnUpdateStance.TryInvoke(rp, (byte) stance.stance);
        }

        public delegate void PlayerUpdatePosition(UnturnedPlayer player, Vector3 position);
        public static event PlayerUpdatePosition OnPlayerUpdatePosition;
        internal static void fireOnPlayerUpdatePosition(UnturnedPlayer player)
        {
            OnPlayerUpdatePosition.TryInvoke(player,player.Position);
        }

        public delegate void PlayerUpdateBleeding(UnturnedPlayer player, bool bleeding);
        public static event PlayerUpdateBleeding OnPlayerUpdateBleeding;
        public event PlayerUpdateBleeding OnUpdateBleeding;

        public delegate void PlayerUpdateBroken(UnturnedPlayer player, bool broken);
        public static event PlayerUpdateBroken OnPlayerUpdateBroken;
        public event PlayerUpdateBroken OnUpdateBroken;

        public delegate void PlayerDeath(UnturnedPlayer player, EDeathCause cause, ELimb limb, CSteamID murderer);
        public static event PlayerDeath OnPlayerDeath;
        public event PlayerDeath OnDeath;

        public delegate void PlayerDead(UnturnedPlayer player, Vector3 position);
        public static event PlayerDead OnPlayerDead;
        public event PlayerDead OnDead;

        public delegate void PlayerUpdateLife(UnturnedPlayer player, byte life);
        public static event PlayerUpdateLife OnPlayerUpdateLife;
        public event PlayerUpdateLife OnUpdateLife;

        public delegate void PlayerUpdateFood(UnturnedPlayer player, byte food);
        public static event PlayerUpdateFood OnPlayerUpdateFood;
        public event PlayerUpdateFood OnUpdateFood;

        public delegate void PlayerUpdateHealth(UnturnedPlayer player, byte health);
        public static event PlayerUpdateHealth OnPlayerUpdateHealth;
        public event PlayerUpdateHealth OnUpdateHealth;

        public delegate void PlayerUpdateVirus(UnturnedPlayer player, byte virus);
        public static event PlayerUpdateVirus OnPlayerUpdateVirus;
        public event PlayerUpdateVirus OnUpdateVirus;

        public delegate void PlayerUpdateWater(UnturnedPlayer player, byte water);
        public static event PlayerUpdateWater OnPlayerUpdateWater;
        public event PlayerUpdateWater OnUpdateWater;

        public enum PlayerGesture { None = 0, InventoryOpen = 1, InventoryClose = 2, Pickup = 3, PunchLeft = 4, PunchRight = 5, SurrenderStart = 6, SurrenderStop = 7, Point = 8, Wave = 9 , Salute = 10 , Arrest_Start = 11 , Arrest_Stop = 12 , Rest_Start = 13 , Rest_Stop = 14 , Facepalm = 15 };
        public delegate void PlayerUpdateGesture(UnturnedPlayer player, PlayerGesture gesture);
        public static event PlayerUpdateGesture OnPlayerUpdateGesture;
        public event PlayerUpdateGesture OnUpdateGesture;

        public delegate void PlayerUpdateStance(UnturnedPlayer player, byte stance);
        public static event PlayerUpdateStance OnPlayerUpdateStance;
        public event PlayerUpdateStance OnUpdateStance;

        public delegate void PlayerRevive(UnturnedPlayer player, Vector3 position, byte angle);
        public static event PlayerRevive OnPlayerRevive;
        public event PlayerRevive OnRevive;

        public delegate void PlayerUpdateStat(UnturnedPlayer player, EPlayerStat stat);
        public static event PlayerUpdateStat OnPlayerUpdateStat;
        public event PlayerUpdateStat OnUpdateStat;

        public delegate void PlayerUpdateExperience(UnturnedPlayer player, uint experience);
        public static event PlayerUpdateExperience OnPlayerUpdateExperience;
        public event PlayerUpdateExperience OnUpdateExperience;

        public delegate void PlayerUpdateStamina(UnturnedPlayer player, byte stamina);
        public static event PlayerUpdateStamina OnPlayerUpdateStamina;
        public event PlayerUpdateStamina OnUpdateStamina;

        private void onUpdateStamina(byte stamina)
        {
            OnPlayerUpdateStamina.TryInvoke(Player, stamina);
            OnUpdateStamina.TryInvoke(Player, stamina);
        }

        public delegate void PlayerInventoryUpdated(UnturnedPlayer player, InventoryGroup inventoryGroup, byte inventoryIndex, ItemJar P);
        public static event PlayerInventoryUpdated OnPlayerInventoryUpdated;
        public event PlayerInventoryUpdated OnInventoryUpdated;

        private void onInventoryUpdated(byte E, byte O, ItemJar P)
        {
            OnPlayerInventoryUpdated.TryInvoke(Player, (InventoryGroup)Enum.Parse(typeof(InventoryGroup), E.ToString()), O, P);
            OnInventoryUpdated.TryInvoke(Player, (InventoryGroup)Enum.Parse(typeof(InventoryGroup), E.ToString()), O, P);
        }

        public delegate void PlayerInventoryResized(UnturnedPlayer player, InventoryGroup inventoryGroup, byte O, byte U);
        public static event PlayerInventoryResized OnPlayerInventoryResized;
        public event PlayerInventoryResized OnInventoryResized;

        private void onInventoryResized(byte E, byte M, byte U)
        {
            OnPlayerInventoryResized.TryInvoke(Player, (InventoryGroup)Enum.Parse(typeof(InventoryGroup), E.ToString()), M, U);
            OnInventoryResized.TryInvoke(Player, (InventoryGroup)Enum.Parse(typeof(InventoryGroup), E.ToString()), M, U);
        }

        public delegate void PlayerInventoryRemoved(UnturnedPlayer player, InventoryGroup inventoryGroup, byte inventoryIndex, ItemJar P);
        public static event PlayerInventoryRemoved OnPlayerInventoryRemoved;
        public event PlayerInventoryRemoved OnInventoryRemoved;

        private void onInventoryRemoved(byte E, byte y, ItemJar f)
        {
            OnPlayerInventoryRemoved.TryInvoke(Player, (InventoryGroup)Enum.Parse(typeof(InventoryGroup), E.ToString()), y, f);
            OnInventoryRemoved.TryInvoke(Player, (InventoryGroup)Enum.Parse(typeof(InventoryGroup), E.ToString()), y, f);
        }

        public delegate void PlayerInventoryAdded(UnturnedPlayer player, InventoryGroup inventoryGroup, byte inventoryIndex, ItemJar P);
        public static event PlayerInventoryAdded OnPlayerInventoryAdded;
        public event PlayerInventoryAdded OnInventoryAdded;

        private void onInventoryAdded(byte E, byte u, ItemJar J)
        {
            OnPlayerInventoryAdded.TryInvoke(Player,(InventoryGroup)Enum.Parse(typeof(InventoryGroup), E.ToString()), u, J);
            OnInventoryAdded.TryInvoke(Player, (InventoryGroup)Enum.Parse(typeof(InventoryGroup), E.ToString()), u, J);
        }

        public delegate void PlayerChatted(UnturnedPlayer player, ref Color color, string message, EChatMode chatMode, ref bool cancel);
        public static event PlayerChatted OnPlayerChatted;

        internal static Color firePlayerChatted(UnturnedPlayer player, EChatMode chatMode, Color color, string msg, ref bool cancel)
        {
            if (OnPlayerChatted != null)
            {
                foreach (var handler in OnPlayerChatted.GetInvocationList().Cast<PlayerChatted>())
                {
                    try
                    {
                        handler(player, ref color, msg, chatMode, ref cancel);
                    }
                    catch (Exception ex)
                    {
                        Core.Logging.Logger.LogException(ex);
                    }
                }
            }

            return color;
        }

        public enum Wearables { Hat = 0, Mask = 1, Vest = 2, Pants = 3, Shirt = 4, Glasses = 5, Backpack = 6};
        public delegate void PlayerWear(UnturnedPlayer player, Wearables wear, ushort id, byte? quality);
        public static event PlayerWear OnPlayerWear;

    }
}
