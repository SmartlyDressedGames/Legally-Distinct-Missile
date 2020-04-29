using Rocket.API;
using System.Xml.Serialization;

namespace Rocket.Unturned.Serialisation
{
    public sealed class AutomaticSaveSettings
    {
        [XmlAttribute]
        public bool Enabled = true;

        [XmlAttribute]
        public uint Interval = 1800;
    }

    public sealed class RocketModObservatorySettings
    {
        [XmlAttribute]
        public bool CommunityBans = true;

        [XmlAttribute]
        public bool KickLimitedAccounts = true;

        [XmlAttribute]
        public bool KickTooYoungAccounts = true;

        [XmlAttribute]
        public ulong MinimumAge = 604800;
    }

    public class UnturnedSettings : IDefaultable
    {
        [XmlElement("AutomaticSave")] 
        public AutomaticSaveSettings AutomaticSave;

        [XmlElement("CharacterNameValidation")]
        public bool CharacterNameValidation;

        [XmlElement("CharacterNameValidationRule")]
        public string CharacterNameValidationRule;

        public bool LogSuspiciousPlayerMovement;

        public bool EnableItemBlacklist;

        public bool EnableItemSpawnLimit;

        public int MaxSpawnAmount;

        public bool EnableVehicleBlacklist;


        public void LoadDefaults()
        {
            AutomaticSave = new AutomaticSaveSettings();
            CharacterNameValidation = true;
            CharacterNameValidationRule = @"([\x00-\xAA]|[\w_\ \.\+\-])+";
            LogSuspiciousPlayerMovement = true;
            EnableItemBlacklist = false;
            EnableItemSpawnLimit = false;
            MaxSpawnAmount = 10;
            EnableVehicleBlacklist = false;
        }
    }
}
