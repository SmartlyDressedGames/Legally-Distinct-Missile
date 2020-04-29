using System;
using System.Linq;
using Rocket.API;
using Rocket.Core;
using Rocket.Core.Steam;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Events;
using Rocket.Unturned.Skills;
using SDG.Unturned;
using Steamworks;
using UnityEngine;

namespace Rocket.Unturned.Player
{
    public class PlayerIsConsoleException : Exception
    {
    }

    public sealed class UnturnedPlayer : IRocketPlayer
    {

        public string Id
        {
            get
            {
                return CSteamID.ToString();
            }
        }

        public string DisplayName
        {
            get
            {
                return CharacterName;
            }
        }

        public bool IsAdmin
        {
            get
            {
                return _player.channel.owner.isAdmin;
            }
        }

        public Profile SteamProfile
        {
            get { return new Profile(ulong.Parse(CSteamID.ToString())); }
        }

        private readonly SDG.Unturned.Player _player;
        public SDG.Unturned.Player Player
        {
            get { return _player; }
        }

        public CSteamID CSteamID
        {
            get { return _player.channel.owner.playerID.steamID; }
        }

        public Exception PlayerIsConsoleException;

        private UnturnedPlayer(SteamPlayer player)
        {
            this._player = player.player;
        }

        public Color Color
        {
            get
            {
                if (Features.Color.HasValue)
                {
                    return Features.Color.Value;
                }
                if (IsAdmin && !Provider.hideAdmins)
                {
                    return Palette.ADMIN;
                }

                var group = R.Permissions.GetGroups(this, false).FirstOrDefault(g => g.Color != null && g.Color != "white");
                var color = "";
                if (group != null) color = group.Color;
                return UnturnedChat.GetColorFromName(color, Palette.COLOR_W);
            }
            set
            {
                Features.Color = value;
            }
        }


        private UnturnedPlayer(CSteamID cSteamID)
        {
            if (string.IsNullOrEmpty(cSteamID.ToString()) || cSteamID.ToString() == "0")
            {
                throw new PlayerIsConsoleException();
            }

            _player = PlayerTool.getPlayer(cSteamID);
        }

        public float Ping
        {
            get
            {
                return _player.channel.owner.ping;
            }
        }

        public bool Equals(UnturnedPlayer p)
        {
            if (p == null)
            {
                return false;
            }

            return CSteamID.ToString() == p.CSteamID.ToString();
        }

        public T GetComponent<T>()
        {
            return (T)(object)Player.GetComponent(typeof(T));
        }

        private UnturnedPlayer(SDG.Unturned.Player player)
        {
            _player = player;
        }

        public static UnturnedPlayer FromName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }

            SDG.Unturned.Player player;
            if (ulong.TryParse(name, out var id) && id > 76561197960265728)
            {
                player = PlayerTool.getPlayer(new CSteamID(id));
            }
            else
            {
                player = PlayerTool.getPlayer(name);
            }

            return player != null ? new UnturnedPlayer(player) : null;
        }

        public static UnturnedPlayer FromCSteamID(CSteamID steamId)
        {
            if (string.IsNullOrEmpty(steamId.ToString()) || steamId.ToString() == "0")
            {
                return null;
            }

            return new UnturnedPlayer(steamId);
        }

        public static UnturnedPlayer FromPlayer(SDG.Unturned.Player player)
        {
            return new UnturnedPlayer(player.channel.owner);
        }

        public static UnturnedPlayer FromSteamPlayer(SteamPlayer player)
        {
            return new UnturnedPlayer(player);
        }

        public UnturnedPlayerFeatures Features
        {
            get { return _player.gameObject.transform.GetComponent<UnturnedPlayerFeatures>(); }
        }

        public UnturnedPlayerEvents Events
        {
            get { return _player.gameObject.transform.GetComponent<UnturnedPlayerEvents>(); }
        }

        public override string ToString()
        {
            return CSteamID.ToString();
        }

        public void TriggerEffect(ushort effectId)
        {
            EffectManager.instance.channel.send("tellEffectPoint", CSteamID, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, effectId, _player.transform.position);
        }
        
        public string IP
        {
            get
            {
                SteamGameServerNetworking.GetP2PSessionState(CSteamID, out var state);
                return Parser.getIPFromUInt32(state.m_nRemoteIP);
            }
        }

        public void MaxSkills()
        {
            var skills = _player.skills;
            
            foreach (var skill in skills.skills.SelectMany(s => s))
            {
                skill.level = skill.max;
            }
            
            skills.askSkills(_player.channel.owner.playerID.steamID);
        }

        public string SteamGroupName()
        {
            FriendsGroupID_t id;
            id.m_FriendsGroupID = (short)SteamGroupID.m_SteamID;
            return SteamFriends.GetFriendsGroupName(id);
        }

        public int SteamGroupMembersCount()
        {
            FriendsGroupID_t id;
            id.m_FriendsGroupID = (short)SteamGroupID.m_SteamID;
            return SteamFriends.GetFriendsGroupMembersCount(id);
        }

        public SteamPlayer SteamPlayer()
        {
            return Provider.clients.FirstOrDefault(s => CSteamID == s.playerID.steamID);
        }

        public PlayerInventory Inventory
        {
            get { return _player.inventory; }
        }

        public bool GiveItem(ushort itemId, byte amount)
        {
            return ItemTool.tryForceGiveItem(_player, itemId, amount);
        }

        public bool GiveItem(Item item)
        {
            return _player.inventory.tryAddItem(item, false);
        }

        public bool GiveVehicle(ushort vehicleId)
        {
            return VehicleTool.giveVehicle(_player, vehicleId);
        }

        public CSteamID SteamGroupID
        {
            get
            {
                return _player.channel.owner.playerID.group;
            }
        }

        public void Kick(string reason)
        {
            Provider.kick(CSteamID, reason);
        }

        public void Ban(string reason, uint duration)
        {
            Provider.ban(CSteamID, reason, duration);
        }

        public void Admin(bool admin)
        {
            Admin(admin, null);
        }

        public void Admin(bool admin, UnturnedPlayer issuer)
        {
            if (admin)
            {
                SteamAdminlist.admin(CSteamID, issuer?.CSteamID ?? new CSteamID(0));
            }
            else
            {
                SteamAdminlist.unadmin(_player.channel.owner.playerID.steamID);
            }
        }

        public void Teleport(UnturnedPlayer target)
        {
            var position = target._player.transform.position;
            var rotation = target._player.transform.rotation.eulerAngles;

            Teleport(position, MeasurementTool.angleToByte(rotation.y));
        }

        public void Teleport(Vector3 position, float rotation)
        {
            if (VanishMode)
            {
                _player.channel.send("askTeleport", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER, position, MeasurementTool.angleToByte(rotation));
                _player.channel.send("askTeleport", ESteamCall.NOT_OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new Vector3(position.y, position.y + 1337, position.z), MeasurementTool.angleToByte(rotation));
                _player.channel.send("askTeleport", ESteamCall.SERVER, ESteamPacket.UPDATE_RELIABLE_BUFFER, position, MeasurementTool.angleToByte(rotation));
            }
            else
            {
				_player.teleportToLocation(position, rotation);
            }
        }

        public bool VanishMode
        {
            get
            {
                var features = _player.GetComponent<UnturnedPlayerFeatures>();
                return features.VanishMode;
            }
            set
            {
                var features = _player.GetComponent<UnturnedPlayerFeatures>();
                features.VanishMode = value;
            }
        }

        public bool GodMode
        {
            get
            {
                var features = _player.GetComponent<UnturnedPlayerFeatures>();
                return features.GodMode;
            }
            set
            {
                var features = _player.GetComponent<UnturnedPlayerFeatures>();
                features.GodMode = value;
            }
        }

        public Vector3 Position
        {
            get
            {
                return _player.transform.position;
            }
        }

        public EPlayerStance Stance
        {
            get
            {
                return _player.stance.stance;
            }
        }

        public float Rotation
        {
            get
            {
                return _player.transform.rotation.eulerAngles.y;
            }
        }

        public bool Teleport(string nodeName)
        {
            var node = LevelNodes.nodes.FirstOrDefault(n => n.type == ENodeType.LOCATION && ((LocationNode)n).name.ToLower().Contains(nodeName));
            if (node != null)
            {
                var position = node.point + new Vector3(0f, 0.5f, 0f);
                _player.sendTeleport(position, MeasurementTool.angleToByte(Rotation));
                return true;
            }

            return false;
        }

        public byte Stamina
        {
            get
            {
                return _player.life.stamina;
            }
        }

        public string CharacterName
        {
            get
            {
                return _player.channel.owner.playerID.characterName;
            }
        }

        public string SteamName
        {
            get
            {
                return _player.channel.owner.playerID.playerName;
            }
        }

        public byte Infection
        {
            get
            {
                return _player.life.virus;
            }
            set
            {
                _player.life.askDisinfect(100);
                _player.life.askInfect(value);
            }
        }

        public uint Experience
        {
            get
            {
                return _player.skills.experience;
            }
            set
            {
                _player.skills.channel.send("tellExperience", ESteamCall.SERVER, ESteamPacket.UPDATE_RELIABLE_BUFFER, value);
                _player.skills.channel.send("tellExperience", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER, value);
            }
        }

        public int Reputation
        {
            get
            {
                return _player.skills.reputation;
            }
            set
            {
                _player.skills.askRep(value);
            }
        }

        public byte Health
        {
            get
            {
                return _player.life.health;
            }
        }

        public byte Hunger
        {
            get
            {
                return _player.life.food;
            }
            set
            {
                _player.life.askEat(100);
                _player.life.askStarve(value);
            }
        }

        public byte Thirst
        {
            get
            {
                return _player.life.water;
            }
            set
            {
                _player.life.askDrink(100);
                _player.life.askDehydrate(value);
            }
        }

        public bool Broken
        {
            get
            {
                return _player.life.isBroken;
            }
            set
            {
                _player.life.tellBroken(Provider.server,value);
                _player.life.channel.send("tellBroken", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER, value);
            }
        }
        public bool Bleeding
        {
            get
            {
                return _player.life.isBleeding;
            }
            set
            {
                _player.life.tellBleeding(Provider.server, value);
                _player.life.channel.send("tellBleeding", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER, value);
            }
        }

        public bool Dead
        {
            get
            {
                return _player.life.isDead;
            }
        }

        public void Heal(byte amount)
        {
            Heal(amount, null, null);
        }

        public void Heal(byte amount, bool? bleeding, bool? broken)
        {
            _player.life.askHeal(amount, bleeding ?? _player.life.isBleeding, broken ?? _player.life.isBroken);
        }

        public void Suicide()
        {
            _player.life.askSuicide(_player.channel.owner.playerID.steamID);
        }

        public EPlayerKill Damage(byte amount, Vector3 direction, EDeathCause cause, ELimb limb, CSteamID damageDealer)
        {
            _player.life.askDamage(amount, direction, cause, limb, damageDealer, out var playerKill);
            return playerKill;
        }

        public bool IsPro
        {
            get
            {
                return _player.channel.owner.isPro;
            }
        }

        public InteractableVehicle CurrentVehicle
        {
            get
            {
                return _player.movement.getVehicle();
            }
        }

        public bool IsInVehicle
        {
            get
            {
                return CurrentVehicle != null;
            }
        }

        public void SetSkillLevel(UnturnedSkill skill, byte level)
        {
            GetSkill(skill).level = level;
            _player.skills.askSkills(CSteamID);
        }

        public byte GetSkillLevel(UnturnedSkill skill)
        {
            return GetSkill(skill).level;
        }

        public Skill GetSkill(UnturnedSkill skill)
        {
            var skills = _player.skills;
            return skills.skills[skill.Speciality][skill.Skill];
        }

        public int CompareTo(object obj)
        {
            return Id.CompareTo(obj);
        }
    }
}