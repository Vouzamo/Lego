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
    public class Hub
    {
        public string DeviceId { get; }
        protected IConnection Connection { get; }
        public IDictionary<byte, IODevice> ConnectedDevices { get; set; } = new ConcurrentDictionary<byte, IODevice>();

        public Queue<IMessage> Messages { get; } = new Queue<IMessage>();

        public Hub(IConnection connection, string deviceId)
        {
            DeviceId = deviceId;

            Connection = connection;

            Connection.OnMessageReceived += ProcessMessage;
        }

        private void ProcessMessage(object sender, IMessage message)
        {
            if(message.MessageType == MessageType.Hub__Attached_IO)
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
            else
            {
                Messages.Enqueue(message);
            }
        }

        public void SendMessage(IMessage message)
        {
            Connection.SendMessage(DeviceId, message);
        }

        public async Task<T> EstablishDeviceConnectionByInterface<T, TType>() where T : TType where TType : IODevice
        {
            while (!ConnectedDevices.OfType<TType>().Any())
            {
                await Task.Delay(250);
            }

            return (T)ConnectedDevices.OfType<TType>().First();
        }

        public async Task<T> EstablishDeviceConnectionByPort<T>(byte port) where T : IODevice
        {
            while(!ConnectedDevices.ContainsKey(port))
            {
                await Task.Delay(250);
            }

            return (T)ConnectedDevices[port];
        }
    }
}
