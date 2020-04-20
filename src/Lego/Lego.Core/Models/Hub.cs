using Lego.Core.Models.Devices.General;
using Lego.Core.Models.Devices.Parts;
using Lego.Core.Models.Messaging;
using Lego.Core.Models.Messaging.Messages;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lego.Core
{
    public abstract class Hub
    {
        public bool IsConnected { get; protected set; } = false;
        protected IConnection Connection { get; set; }
        public IDictionary<byte, IDevice> ConnectedDevices { get; set; } = new ConcurrentDictionary<byte, IDevice>();

        public Queue<IMessage> Messages { get; } = new Queue<IMessage>();

        public Hub(IConnection connection)
        {
            Connection = connection;
        }

        public async Task Connect()
        {
            await Connection.Connect(this);
        }

        public void ReceiveMessage(IMessage message)
        {
            if (message.MessageType == MessageType.Hub__Attached_IO)
            {
                var attachedIOMessage = new AttachedIOMessage(message.Bytes.ToArray());

                switch(attachedIOMessage.Event)
                {
                    case IOEvent.Attached_IO:
                        switch(attachedIOMessage.DeviceType)
                        {
                            case IODeviceType.TechnicMotorL:
                                ConnectedDevices[attachedIOMessage.Port] = new TechnicMotorL(this, attachedIOMessage.Port);
                                break;
                            case IODeviceType.TechnicMotorXL:
                                ConnectedDevices[attachedIOMessage.Port] = new TechnicMotorXL(this, attachedIOMessage.Port);
                                break;
                        }
                        break;
                    case IOEvent.Attached_Virtual_IO:
                        switch (attachedIOMessage.DeviceType)
                        {
                            case IODeviceType.LED_Light:
                                ConnectedDevices[attachedIOMessage.Port] = new LED(this, attachedIOMessage.Port);
                                break;
                        }
                        break;
                }
            }
            else if(message.MessageType == MessageType.Port_Information)
            {
                var portInformationMessage = new PortInformationMessage(message.Bytes.ToArray());

                if (ConnectedDevices.ContainsKey(portInformationMessage.Port))
                {
                    ConnectedDevices[portInformationMessage.Port].ReceiveMessage(portInformationMessage);
                }
            }
            else if (message.MessageType == MessageType.Port_Mode_Information)
            {
                var portModeInformationMessage = new PortModeInformationMessage(message.Bytes.ToArray());

                if (ConnectedDevices.ContainsKey(portModeInformationMessage.Port))
                {
                    ConnectedDevices[portModeInformationMessage.Port].ReceiveMessage(portModeInformationMessage);
                }
            }
            else if(message.MessageType == MessageType.Port_Input_Format__Single)
            {
                var portInputFormatMessage = new PortInputFormatSingleMessage(message.Bytes.ToArray());

                if (ConnectedDevices.ContainsKey(portInputFormatMessage.Port))
                {
                    ConnectedDevices[portInputFormatMessage.Port].ReceiveMessage(portInputFormatMessage);
                }
            }
            else if(message.MessageType == MessageType.Port_Input_Format__Combined_Mode)
            {
                var portInputFormatMessage = new PortInputFormatCombinedMessage(message.Bytes.ToArray());

                if (ConnectedDevices.ContainsKey(portInputFormatMessage.Port))
                {
                    ConnectedDevices[portInputFormatMessage.Port].ReceiveMessage(portInputFormatMessage);
                }
            }
            else if(message.MessageType == MessageType.Port_Value__Single)
            {
                var portValueMessage = new PortValueSingleMessage(message.Bytes.ToArray());

                if(ConnectedDevices.ContainsKey(portValueMessage.Port))
                {
                    ConnectedDevices[portValueMessage.Port].ReceiveMessage(portValueMessage);
                }
            }
            else if (message.MessageType == MessageType.Port_Value__Combined_Mode)
            {
                var portValueMessage = new PortValueCombinedMessage(message.Bytes.ToArray());

                if (ConnectedDevices.ContainsKey(portValueMessage.Port))
                {
                    ConnectedDevices[portValueMessage.Port].ReceiveMessage(portValueMessage);
                }
            }
            else
            {
                Messages.Enqueue(message);
            }
        }

        public void SendMessage(IMessage message)
        {
            Connection.SendMessage(message);
        }

        public async Task<T> EstablishDeviceConnectionByPort<T>(byte port) where T : IDevice
        {
            while(!ConnectedDevices.ContainsKey(port) || !ConnectedDevices[port].IsReady)
            {
                await Task.Delay(250);
            }

            return (T)ConnectedDevices[port];
        }
    }
}
