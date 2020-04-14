using Lego.Core.Models.Messaging;
using Lego.Core.Models.Messaging.Messages;
using System.Linq;
using System.Threading.Tasks;

namespace Lego.Core
{
    public class Device : IDevice
    {
        public Hub Hub { get; }
        public byte Port { get; }
        public PortCapabilities Capabilities { get; protected set; }
        public byte InputMode { get; protected set; }

        public Device(Hub hub, byte port)
        {
            Hub = hub;
            Port = port;

            SendMessage(new PortInformationRequestMessage(Port, PortInformationType.Mode_Information));
        }

        public void SendMessage(IMessage message)
        {
            Hub.SendMessage(message);
        }

        public virtual void ReceiveMessage(IMessage message)
        {
            switch(message)
            {
                case PortValueMessage portValueMessage:
                    HandleValue(portValueMessage.Body.ToArray());
                    break;
                case PortInformationMessage portInformationMessage:
                    switch(portInformationMessage.InformationType)
                    {
                        case PortInformationType.Mode_Information:
                            Capabilities = (PortCapabilities)portInformationMessage.Body.ElementAt(2);
                            break;
                    }
                    break;
                case PortInputFormatMessage portInputFormatMessage:
                    InputMode = portInputFormatMessage.Mode;

                    SendMessage(new PortInformationRequestMessage(Port, PortInformationType.Value));
                    break;
            }
        }

        public void SetInputModes(byte[] modes, uint delta = 1, bool notify = true)
        {
            if (Capabilities.HasFlag(PortCapabilities.Input))
            {
                if (Capabilities.HasFlag(PortCapabilities.Combinable) && modes.Length > 1)
                {
                    // combined input
                }
                else
                {
                    // single input
                    SendMessage(new PortInputFormatSetupMessage(Port, modes[0], delta, notify));
                }
            }
        }

        public virtual void HandleValue(byte[] bytes)
        {

        }
    }
}
