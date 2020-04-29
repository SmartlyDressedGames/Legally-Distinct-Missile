using System.Xml.Serialization;

namespace Rocket.Unturned.Serialisation
{
    public sealed class RocketModObservatorySettings
    {
        [XmlAttribute]
        public bool CommunityBans = true;

        [XmlAttribute]
        public bool KickLimitedAccounts = true;

        [XmlAttribute]
        public bool KickTooYoungAccounts = true;

        [XmlAttribute]
        public long MinimumAge = 604800;
    }
}