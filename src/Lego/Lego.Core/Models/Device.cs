namespace Lego.Core
{
    public class Device : IODevice
    {
        public Hub Hub { get; }
        public byte Port { get; }

        public Device(Hub hub, byte port)
        {
            Hub = hub;
            Port = port;
        }

        public void SendMessage(IMessage message)
        {
            Hub.SendMessage(message);
        }
    }
}
