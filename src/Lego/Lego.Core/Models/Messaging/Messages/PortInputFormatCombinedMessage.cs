using System.Linq;

namespace Lego.Core.Models.Messaging.Messages
{
    public class PortInputFormatCombinedMessage : Message
    {
        public byte Port => Body.ElementAt(0);

        public PortInputFormatCombinedMessage(byte[] bytes) : base(bytes)
        {

        }
    }
}
