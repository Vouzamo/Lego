using System;
using System.Linq;

namespace Lego.Core.Models.Messaging.Messages
{
    public class PortInputFormatSingleMessage : Message
    {
        public byte Port => Body.ElementAt(0);
        public byte Mode => Body.ElementAt(1);
        public uint Delta => BitConverter.ToUInt32(Body.ToArray(), 2);
        public bool Notify => BitConverter.ToBoolean(Body.ToArray(), 6);

        public PortInputFormatSingleMessage(byte[] bytes) : base(bytes)
        {

        }
    }
}
