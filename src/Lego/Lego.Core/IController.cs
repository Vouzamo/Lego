using Lego.Core.Extensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lego.Core
{
    public interface IConnection
    {
        Task<Hub> EstablishHubConnectionById(string deviceId);

        event EventHandler<IMessage> OnMessageReceived;

        void SendMessage(string deviceId, IMessage message);
    }

    public interface IMessage
    {
        IEnumerable<byte> Bytes { get; }
        IEnumerable<byte> CommonHeader { get; }
        IEnumerable<byte> Body { get; }

        ushort MessageLength { get; }
        byte HubID { get; }
        MessageType MessageType { get; }
    }

    public class Message : IMessage
    {
        public IEnumerable<byte> Bytes { get; protected set; }
        public IEnumerable<byte> CommonHeader => Bytes.GetRange(0, LengthOffset + 2);
        public IEnumerable<byte> Body => Bytes.GetRange(CommonHeader.Count(), MessageLength - CommonHeader.Count());

        protected byte LengthOffset => (Bytes.First() & 0b10000000) == 0 ? (byte)1 : (byte)2;
        public ushort MessageLength => DetermineLength();

        public byte HubID => Bytes.ElementAt(LengthOffset);

        public MessageType MessageType => (MessageType)Bytes.ElementAt(LengthOffset + 1);

        public Message(byte[] bytes)
        {
            Bytes = bytes;
        }

        private ushort DetermineLength()
        {
            ushort length = Bytes.First();

            if (LengthOffset == 1)
            {
                return length;
            }

            ushort overflow = (ushort)(Bytes.ElementAt(1) << 7);

            return (ushort)(length | overflow);
        }
    }

    public class AttachedIOMessage : Message
    {
        public byte Port => Body.ElementAt(0);
        public IOEvent Event => (IOEvent)Body.ElementAt(1);

        public IODeviceType DeviceType => Event != IOEvent.Detached_IO ? (IODeviceType)BitConverter.ToUInt16(Body.ToArray(), 2) : IODeviceType.Unknown;
        public VersionNumber HardwareRevision => Event == IOEvent.Attached_IO ? new VersionNumber(Body.GetRange(4, 4)) : null;
        public VersionNumber SoftwareRevision => Event == IOEvent.Attached_IO ? new VersionNumber(Body.GetRange(8, 4)) : null;
        public byte PortID_A => Event == IOEvent.Attached_Virtual_IO ? Body.ElementAt(4) : (byte)0;
        public byte PortID_B => Event == IOEvent.Attached_Virtual_IO ? Body.ElementAt(5) : (byte)0;

        public AttachedIOMessage(byte[] bytes) : base(bytes)
        {

        }
    }

    public enum MessageType : byte
    {
        Hub__Properties = 0x01,
        Hub__Actions = 0x02,
        Hub__Alerts = 0x03,
        Hub__Attached_IO = 0x04,
        Generic_Error_Messages = 0x05,
        Hardware_Network_Commands = 0x08,
        Firmware_Update__Go_Into_Boot_Mode = 0x10,
        Firmware_Update__Lock_Memory = 0x11,
        Firmware_Update__Lock_Status_Request = 0x12,
        Firmware_Lock_Status = 0x13,
        Port_Information_Request = 0x21,
        Port_Mode_Information_Request = 0x22,
        Port_Input_Format_Setup__Single = 0x41,
        Port_Input_Format_Setup__Combined_Mode = 0x42,
        Port_Information = 0x43,
        Port_Mode_Information = 0x44,
        Port_Value__Single = 0x45,
        Port_Value__Combined_Mode = 0x46,
        Port_Input_Format__Single = 0x47,
        Port_Input_Format__Combined_Mode = 0x48,
        Virtual_Port_Setup = 0x61,
        Port_Output_Command = 0x81,
        Port_Output_Command_Feedback = 0x82
    }

    public class VersionNumber
    {
        public byte Major { get; }
        public byte Minor { get; }
        public byte Patch { get; }
        public ushort Build { get; }

        protected IEnumerable<byte> Bytes { get; }

        public VersionNumber(IEnumerable<byte> bytes)
        {
            Bytes = bytes;
        }
    }

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
                                ConnectedDevices[attachedIOMessage.Port] = new TechnicMotor(this, attachedIOMessage.Port, "L");
                                break;
                            case IODeviceType.TechnicMotorXL:
                                ConnectedDevices[attachedIOMessage.Port] = new TechnicMotor(this, attachedIOMessage.Port, "XL");
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

    public interface IODevice
    {
        Hub Hub { get; }
        byte Port { get; }

        void SendMessage(IMessage message);
    }

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

    public interface ILightEmittingDiode : IODevice
    {

    }

    public interface IMotor : IODevice
    {
        
    }

    public class TechnicMotor : Device, IMotor
    {
        public string Size { get; }

        public TechnicMotor(Hub hub, byte port, string size) : base(hub, port)
        {
            Size = size;
        }
    }

    public class LED : Device, ILightEmittingDiode
    {
        public LED(Hub hub, byte port) : base(hub, port)
        {

        }
    }

    public enum IOEvent : byte
    {
        Detached_IO = 0x00,
        Attached_IO = 0x01,
        Attached_Virtual_IO = 0x02
    }

    public enum IODeviceType : ushort
    {
        Unknown = 0x0000,
        Motor = 0x0001,
        System_Train_Motor = 0x0002,
        Button = 0x0005,
        LED_Light = 0x0008,
        Voltage = 0x0014,
        Current = 0x0015,
        Piezo_Tone_ = 0x0016,
        RGB_Light = 0x0017,
        External_Tilt_Sensor = 0x0022,
        Motion_Sensor = 0x0023,
        Vision_Sensor = 0x0025,
        External_Motor_With_Tacho = 0x0026,
        Internal_Motor_With_Tacho = 0x0027,
        Internal_Tilt = 0x0028,
        TechnicMotorL = 0x002E,
        TechnicMotorXL = 0x002F
    }
}
