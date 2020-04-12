using Lego.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Lego.Core.Models.Messaging.Messages
{
    public abstract class DownstreamMessage : Message
    {

        public DownstreamMessage(MessageType type, byte[] payload) : base(PrefixCommonHeader(type, payload))
        {

        }

        public static byte[] PrefixCommonHeader(MessageType type, byte[] payload)
        {
            var bytes = new List<byte>
            {
                (byte)(payload.Length + 2), // todo: handle long messages
                0x00, // hub id
                (byte)type
            };

            bytes.AddRange(payload);

            return bytes.ToArray();
        }
    }

    public class PortInformationRequestMessage : DownstreamMessage
    {
        public PortInformationRequestMessage(byte port, PortInformationType informationType) : base(MessageType.Port_Information_Request, new byte[] { port, (byte)informationType })
        {

        }
    }

    public class PortInputFormatSetupMessage : DownstreamMessage
    {
        public PortInputFormatSetupMessage(byte port, byte mode, uint delta, bool notify) : base(MessageType.Port_Input_Format_Setup__Single, ToByteArray(port, mode, delta, notify))
        {

        }

        public static byte[] ToByteArray(byte port, byte mode, uint delta, bool notify)
        {
            var bytes = new List<byte>
            {
                port,
                mode
            };

            bytes.AddRange(BitConverter.GetBytes(delta));
            bytes.AddRange(BitConverter.GetBytes(notify));

            return bytes.ToArray();
        }
    }

    public class PortInputFormatMessage : Message
    {
        public byte Port => Body.ElementAt(0);
        public byte Mode => Body.ElementAt(1);
        public uint Delta => BitConverter.ToUInt32(Body.ToArray(), 2);
        public bool Notify => BitConverter.ToBoolean(Body.ToArray(), 6);

        public PortInputFormatMessage(byte[] bytes) : base(bytes)
        {

        }
    }

    public class PortInformationMessage : Message
    {
        public byte Port => Body.ElementAt(0);
        public PortInformationType InformationType => (PortInformationType)Body.ElementAt(1);

        public PortInformationMessage(byte[] bytes) : base(bytes)
        {
            
        }
    }

    public class PortValueMessage : Message
    {
        public byte Port => Body.ElementAt(0);

        public PortValueMessage(byte[] bytes) : base(bytes)
        {

        }
    }

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
