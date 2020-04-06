namespace Lego.Core.Models.Messaging
{
    public enum IOEvent : byte
    {
        Detached_IO = 0x00,
        Attached_IO = 0x01,
        Attached_Virtual_IO = 0x02
    }
}
