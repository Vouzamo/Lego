using Lego.Core.Models.Messaging.Messages;
using System.Threading.Tasks;

namespace Lego.Core
{
    public interface IDevice
    {
        Hub Hub { get; }
        byte Port { get; }

        void SetInputModes(byte[] modes, uint delta = 1, bool notify = true);
        void SendMessage(IMessage message);
        void ReceiveMessage(IMessage message);
    }
}
