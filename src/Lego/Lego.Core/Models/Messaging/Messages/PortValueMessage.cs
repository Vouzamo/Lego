using System.Linq;

namespace Lego.Core.Models.Messaging.Messages
{
    public class PortValueSingleMessage : Message
    {
        public byte Port => Body.ElementAt(0);

        public PortValueSingleMessage(byte[] bytes) : base(bytes)
        {

        }
    }

    public class PortValueCombinedMessage : Message
    {
        public byte Port => Body.ElementAt(0);

        public PortValueCombinedMessage(byte[] bytes) : base(bytes)
        {

        }
    }
}
