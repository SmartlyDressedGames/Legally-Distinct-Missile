using Rocket.API;
using System;
using System.Xml.Serialization;

namespace Rocket.Unturned.Serialisation
{
    public sealed class AutomaticSaveSettings
    {
        [XmlAttribute]
        public bool Enabled = true;

        [XmlAttribute]
        public int Interval = 1800;
    }

    public sealed class RocketModObservatorySettings
    {
        [Obsolete("Observatory is no longer maintained.")]
        [XmlAttribute]
        public bool CommunityBans = true;


        [XmlAttribute] 
        public bool KickIfFail;

        [XmlAttribute]
        public bool KickLimitedAccounts = true;

        [XmlAttribute]
        public bool KickPrivateAccounts;

        [XmlAttribute]
        public bool KickTooYoungAccounts = true;

        [XmlAttribute]
        public bool KickVacBanned;

        [XmlAttribute]
        public long MinimumAge = 604800;
    }

    public class UnturnedSettings : IDefaultable
    {
        [XmlElement("RocketModObservatory")] 
        public RocketModObservatorySettings RocketModObservatory;
        [XmlElement("AutomaticSave")] 
        public AutomaticSaveSettings AutomaticSave;


        [XmlElement("CharacterNameValidation")]
        public bool CharacterNameValidation = false;

        [XmlElement("CharacterNameValidationRule")]
        public string CharacterNameValidationRule = @"([\x00-\xAA]|[\w_\ \.\+\-])+";

        public bool LogSuspiciousPlayerMovement = true;

        public bool EnableItemBlacklist;

        public bool EnableItemSpawnLimit;

        public int MaxSpawnAmount;

        public bool EnableVehicleBlacklist;


        public void LoadDefaults()
        {
            AutomaticSave = new AutomaticSaveSettings();
            RocketModObservatory = new RocketModObservatorySettings();
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
