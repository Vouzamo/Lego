using Lego.Core.Extensions;
using System;
using System.Linq;

namespace Lego.Core.Models.Messaging.Messages
{
    public class AttachedIOMessage : Message
    {
        public byte Port => Body.ElementAt(0);
        public IOEvent Event => (IOEvent)Body.ElementAt(1);

        public IODeviceType DeviceType => Event != IOEvent.Detached_IO ? (IODeviceType)BitConverter.ToUInt16(Body.ToArray(), 2) : IODeviceType.Unknown;
        public VersionNumber HardwareRevision => Event == IOEvent.Attached_IO ? new VersionNumber(Body.GetRange(4, 4)) : null;
        public VersionNumber SoftwareRevision => Event == IOEvent.Attached_IO ? new VersionNumber(Body.GetRange(8, 4)) : null;
        public byte PortID_A => Event == IOEvent.Attached_Virtual_IO ? Body.ElementAt(4) : (byte)0;
        public byte PortID_B => Event == IOEvent.Attached_Virtual_IO ? Body.ElementAt(5) : (byte)0;

        public AttachedIOMessage(byte[] bytes) : base(bytes)
        {

        }
    }
}
