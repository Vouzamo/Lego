using System.Collections.Generic;

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
                (byte)(payload.Length + 3), // todo: handle long messages
                0x00, // hub id
                (byte)type
            };

            bytes.AddRange(payload);

            return bytes.ToArray();
        }
    }
}
