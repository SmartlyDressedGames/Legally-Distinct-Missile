namespace Rocket.Unturned.Items
{
    public class Attachment
    {
        public ushort AttachmentId;
        public byte Durability = 100;

        public Attachment(ushort attachmentId, byte durability)
        {
            AttachmentId = attachmentId;
            Durability = durability;
        }
    }
}