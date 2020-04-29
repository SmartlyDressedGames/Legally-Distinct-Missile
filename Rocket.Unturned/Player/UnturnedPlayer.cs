#region

using System.Linq;
using Rocket.API;
using Rocket.Core;
using Rocket.Core.Steam;
using Rocket.Core.Utils;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Events;
using Rocket.Unturned.Skills;
using SDG.Unturned;
using Steamworks;
using UnityEngine;

#endregion

namespace Rocket.Unturned.Player
{
    public sealed class UnturnedPlayer : MonoBehaviour, IRocketPlayer
    {
        public string CharacterName => Player.channel.owner.playerID.characterName;
        public CSteamID CSteamID => Player.channel.owner.playerID.steamID;
        public InteractableVehicle CurrentVehicle => Player.movement.getVehicle();
        public bool Dead => Player.life.isDead;
        public byte Health => Player.life.health;
        public PlayerInventory Inventory => Player.inventory;

        public string IP => SteamGameServerNetworking.GetP2PSessionState(CSteamID, out var state)
            ? Parser.getIPFromUInt32(state.m_nRemoteIP)
            : null;

        public bool IsInVehicle => CurrentVehicle != null;
        public bool IsPro => Player.channel.owner.isPro;
        public float Ping => Player.channel.owner.ping;
        public Vector3 Position => Player.transform.position;
        public SDG.Unturned.Player Player => gameObject.GetComponent<SDG.Unturned.Player>();
        public float Rotation => Player.transform.rotation.eulerAngles.y;
        public byte Stamina => Player.life.stamina;
        public EPlayerStance Stance => Player.stance.stance;
        public CSteamID SteamGroupID => Player.channel.owner.playerID.group;
        public string SteamName => Player.channel.owner.playerID.playerName;
        public Profile SteamProfile => new Profile(ulong.Parse(CSteamID.ToString()));


        public bool Bleeding
        {
            get => Player.life.isBleeding;
            set
            {
                TaskDispatcher.QueueOnMainThread(() =>
                {
                    Player.life.channel.send("tellBleeding", ESteamCall.SERVER, ESteamPacket.UPDATE_RELIABLE_BUFFER,
                        value);
                    Player.life.channel.send("tellBleeding", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER,
                        value);
                });
            }
        }

        public bool Broken
        {
            get => Player.life.isBroken;
            set
            {
                TaskDispatcher.QueueOnMainThread(() =>
                {
                    Player.life.channel.send("tellBroken", ESteamCall.SERVER, ESteamPacket.UPDATE_RELIABLE_BUFFER,
                        value);
                    Player.life.channel.send("tellBroken", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER,
                        value);
                });
            }
        }

        public Color Color
        {
            get
            {
                if (Features.Color.HasValue) return Features.Color.Value;


                if (IsAdmin && !Provider.hideAdmins) return Palette.ADMIN;


                var group = R.Permissions.GetGroups(this, false)
                    .FirstOrDefault(g => g.Color != null && g.Color != "white");
                var color = group != null ? group.Color : string.Empty;
                return UnturnedChat.GetColorFromName(color, Palette.COLOR_W);
            }
            set => Features.Color = value;
        }

        public uint Experience
        {
            get => Player.skills.experience;
            set
            {
                TaskDispatcher.QueueOnMainThread(() =>
                {
                    Player.skills.channel.send("tellExperience", ESteamCall.SERVER, ESteamPacket.UPDATE_RELIABLE_BUFFER,
                        value);
                    Player.skills.channel.send("tellExperience", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER,
                        value);
                });
            }
        }

        public byte Hunger
        {
            get => Player.life.food;
            set
            {
                TaskDispatcher.QueueOnMainThread(() =>
                {
                    Player.life.channel.send("tellFood", ESteamCall.SERVER, ESteamPacket.UPDATE_RELIABLE_BUFFER, value);
                    Player.life.channel.send("tellFood", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER, value);
                });
            }
        }

        public byte Infection
        {
            get => Player.life.virus;
            set
            {
                TaskDispatcher.QueueOnMainThread(() =>
                {
                    Player.life.channel.send("tellVirus", ESteamCall.SERVER, ESteamPacket.UPDATE_RELIABLE_BUFFER,
                        value);
                    Player.life.channel.send("tellVirus", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER, value);
                });
            }
        }

        public bool GodMode
        {
            get => Features.GodMode;
            set => Features.GodMode = value;
        }

        public int Reputation
        {
            get => Player.skills.reputation;
            set => TaskDispatcher.QueueOnMainThread(() => Player.skills.askRep(value));
        }

        public byte Thirst
        {
            get => Player.life.water;
            set
            {
                TaskDispatcher.QueueOnMainThread(() =>
                {
                    Player.life.channel.send("tellWater", ESteamCall.SERVER, ESteamPacket.UPDATE_RELIABLE_BUFFER,
                        value);
                    Player.life.channel.send("tellWater", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER, value);
                });
            }
        }

        public bool VanishMode
        {
            get => Features.VanishMode;
            set => Features.VanishMode = value;
        }


        public UnturnedPlayerEvents Events => Player.gameObject.transform.GetComponent<UnturnedPlayerEvents>();

        public UnturnedPlayerFeatures Features => Player.gameObject.transform.GetComponent<UnturnedPlayerFeatures>();
        public string DisplayName => CharacterName;
        public string Id => CSteamID.ToString();
        public bool IsAdmin => Player.channel.owner.isAdmin;


        public string SteamGroupName()
        {
            FriendsGroupID_t id;
            id.m_FriendsGroupID = (short) SteamGroupID.m_SteamID;
            return SteamFriends.GetFriendsGroupName(id);
        }

        public int SteamGroupMembersCount()
        {
            FriendsGroupID_t id;
            id.m_FriendsGroupID = (short) SteamGroupID.m_SteamID;
            return SteamFriends.GetFriendsGroupMembersCount(id);
        }


        #region Comparation

        public int CompareTo(object obj)
        {
            return Id.CompareTo(obj);
        }

        public bool Equals(UnturnedPlayer p)
        {
            return p != null && CSteamID == p.CSteamID;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as UnturnedPlayer);
        }

        public override int GetHashCode()
        {
            return CSteamID.GetHashCode();
        }

        #endregion


        #region Functions

        public void Admin(bool admin)
        {
            Admin(admin, null);
        }

        public void Admin(bool admin, UnturnedPlayer issuer)
        {
            TaskDispatcher.QueueOnMainThread(() =>
            {
                if (admin)
                    SteamAdminlist.admin(CSteamID, issuer == null ? CSteamID.Nil : issuer.CSteamID);
                else
                    SteamAdminlist.unadmin(CSteamID);
            });
        }

        public void Ban(string reason = null, uint? duration = null, CSteamID? instigator = null)
        {
            TaskDispatcher.QueueOnMainThread(() => Provider.requestBanPlayer(instigator ?? CSteamID.Nil, CSteamID,
                Parser.getUInt32FromIP(IP), reason ?? string.Empty, duration ?? SteamBlacklist.PERMANENT));
        }

        public EPlayerKill Damage(byte amount, Vector3 direction, EDeathCause cause, ELimb limb, CSteamID damageDealer)
        {
            Player.life.askDamage(amount, direction, cause, limb, damageDealer, out var playerKill);
            return playerKill;
        }

        public Skill GetSkill(UnturnedSkill skill)
        {
            var skills = Player.skills;
            return skills.skills[skill.Speciality][skill.Skill];
        }

        public byte GetSkillLevel(UnturnedSkill skill)
        {
            return GetSkill(skill).level;
        }

        public bool GiveItem(ushort itemId, byte amount)
        {
            return ItemTool.tryForceGiveItem(Player, itemId, amount);
        }

        public bool GiveItem(Item item)
        {
            return Player.inventory.tryAddItem(item, false);
        }

        public bool GiveVehicle(ushort vehicleId)
        {
            return VehicleTool.giveVehicle(Player, vehicleId);
        }

        public void Heal(byte amount)
        {
            Heal(amount, null, null);
        }

        public void Heal(byte amount, bool? bleeding, bool? broken)
        {
            TaskDispatcher.QueueOnMainThread(() =>
                Player.life.askHeal(amount, bleeding ?? Player.life.isBleeding, broken ?? Player.life.isBroken));
        }

        public void Kick(string reason)
        {
            TaskDispatcher.QueueOnMainThread(() => Provider.kick(CSteamID, reason));
        }

        public void MaxSkills()
        {
            TaskDispatcher.QueueOnMainThread(() =>
            {
                var skills = Player.skills;
                foreach (var skill in skills.skills.SelectMany(s => s))
                    skill.level = skill.max;

                skills.askSkills(CSteamID);
            });
        }

        public void SetSkillLevel(UnturnedSkill skill, byte level)
        {
            GetSkill(skill).level = level;
            TaskDispatcher.QueueOnMainThread(() => Player.skills.askSkills(CSteamID));
        }

        public SteamPlayer SteamPlayer()
        {
            return Player.channel.owner;
        }

        public void Suicide()
        {
            TaskDispatcher.QueueOnMainThread(() => Player.life.askSuicide(Player.channel.owner.playerID.steamID));
        }

        public void Teleport(UnturnedPlayer target)
        {
            if (target == null)
                return;

            TaskDispatcher.QueueOnMainThread(() => Player.teleportToPlayer(target.Player));
        }

        public void Teleport(Vector3 position, float rotation)
        {
            TaskDispatcher.QueueOnMainThread(() =>
            {
                if (VanishMode)
                {
                    Player.channel.send("askTeleport", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER, position,
                        MeasurementTool.angleToByte(rotation));
                    Player.channel.send("askTeleport", ESteamCall.NOT_OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER,
                        new Vector3(0, position.y + 1337, 0), MeasurementTool.angleToByte(rotation));
                    Player.channel.send("askTeleport", ESteamCall.SERVER, ESteamPacket.UPDATE_RELIABLE_BUFFER, position,
                        MeasurementTool.angleToByte(rotation));
                }
                else
                {
                    Player.teleportToLocation(position, rotation);
                }
            });
        }

        public bool Teleport(string nodeName)
        {
            var node = LevelNodes.nodes.FirstOrDefault(n =>
                n.type == ENodeType.LOCATION && ((LocationNode) n).name.ToLower().Contains(nodeName.ToLower()));
            if (node == null)
                return false;


            var position = node.point;
            position.y = 1024f;
            if (!Physics.Raycast(position, Vector3.down, out var hitInfo, 2048f, RayMasks.WAYPOINT))
                return false;


            position = hitInfo.point + Vector3.up;
            if (!Player.stance.wouldHaveHeightClearanceAtPosition(position, 0.5f))
                return false;


            TaskDispatcher.QueueOnMainThread(() =>
                Player.teleportToLocation(position, MeasurementTool.angleToByte(Rotation)));
            return true;
        }

        public override string ToString()
        {
            return CSteamID.ToString();
        }

        public void TriggerEffect(ushort effectId)
        {
            TaskDispatcher.QueueOnMainThread(() => EffectManager.instance.channel.send("tellEffectPoint", CSteamID,
                ESteamPacket.UPDATE_UNRELIABLE_BUFFER, effectId, Position));
        }

        #endregion


        #region GetUnturnedPlayer

        public static UnturnedPlayer FromName(string name)
        {
            if (string.IsNullOrEmpty(name))
                return null;


            return PlayerTool.tryGetSteamPlayer(name, out var steamPlayer)
                ? steamPlayer.player.gameObject.GetComponent<UnturnedPlayer>()
                : null;
        }

        public static UnturnedPlayer FromCSteamID(CSteamID cSteamId)
        {
            if (cSteamId == CSteamID.Nil || string.IsNullOrEmpty(cSteamId.ToString())) return null;


            return PlayerTool.getSteamPlayer(cSteamId)?.player.gameObject.GetComponent<UnturnedPlayer>();
        }

        public static UnturnedPlayer FromPlayer(SDG.Unturned.Player player)
        {
            return player?.gameObject.GetComponent<UnturnedPlayer>();
        }

        public static UnturnedPlayer FromSteamPlayer(SteamPlayer steamPlayer)
        {
            return steamPlayer?.player.gameObject.GetComponent<UnturnedPlayer>();
        }

        #endregion
    }
}