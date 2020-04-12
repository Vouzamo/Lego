using Lego.Core.Models.Messaging.Messages;

namespace Lego.Core
{
    public interface IDevice
    {
        Hub Hub { get; }
        byte Port { get; }

        void SendMessage(IMessage message);
        void ReceiveMessage(IMessage message);
    }
}
