namespace Lego.Core
{
    public interface IODevice
    {
        Hub Hub { get; }
        byte Port { get; }

        void SendMessage(IMessage message);
    }
}
