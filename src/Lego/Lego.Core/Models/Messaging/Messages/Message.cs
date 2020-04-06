using Lego.Core.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace Lego.Core.Models.Messaging.Messages
{
    public class Message : IMessage
    {
        public IEnumerable<byte> Bytes { get; protected set; }
        public IEnumerable<byte> CommonHeader => Bytes.GetRange(0, LengthOffset + 2);
        public IEnumerable<byte> Body => Bytes.GetRange(CommonHeader.Count(), MessageLength - CommonHeader.Count());

        protected byte LengthOffset => (Bytes.First() & 0b10000000) == 0 ? (byte)1 : (byte)2;
        public ushort MessageLength => DetermineLength();

        public byte HubID => Bytes.ElementAt(LengthOffset);

        public MessageType MessageType => (MessageType)Bytes.ElementAt(LengthOffset + 1);

        public Message(byte[] bytes)
        {
            Bytes = bytes;
        }

        private ushort DetermineLength()
        {
            ushort length = Bytes.First();

            if (LengthOffset == 1)
            {
                return length;
            }

            ushort overflow = (ushort)(Bytes.ElementAt(1) << 7);

            return (ushort)(length | overflow);
        }
    }
}
