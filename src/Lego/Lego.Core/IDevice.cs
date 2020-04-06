namespace Lego.Core
{
    public interface IDevice
    {
        Hub Hub { get; }
        byte Port { get; }

        void SendMessage(IMessage message);
    }
}
