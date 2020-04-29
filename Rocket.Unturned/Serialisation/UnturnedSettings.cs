using System.Xml.Serialization;
using Rocket.API;

namespace Rocket.Unturned.Serialisation
{
    public class UnturnedSettings : IDefaultable
    {
        [XmlElement("RocketModObservatory")]
        public RocketModObservatorySettings RocketModObservatory = new RocketModObservatorySettings();

        [XmlElement("AutomaticSave")]
        public AutomaticSaveSettings AutomaticSave = new AutomaticSaveSettings();

        [XmlElement("CharacterNameValidation")]
        public bool CharacterNameValidation;

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