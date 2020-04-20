using System.Linq;
using System.Text;

namespace Lego.Core.Models.Messaging.Messages
{
    public class PortModeInformationMessage : Message
    {
        public byte Port => Body.ElementAt(0);
        public byte Mode => Body.ElementAt(1);
        public PortModeInformationType InformationType => (PortModeInformationType)Body.ElementAt(2);

        public string Name => Encoding.ASCII.GetString(Body.Skip(3).ToArray());
        public byte[] ValueFormat => Body.Skip(3).ToArray();

        public PortModeInformationMessage(byte[] bytes) : base(bytes)
        {

        }
    }
}
