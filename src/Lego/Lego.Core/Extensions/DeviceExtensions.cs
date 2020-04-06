using Lego.Core.Models.Devices;
using Lego.Core.Models.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Lego.Core.Extensions
{
    public static class DeviceExtensions
    {
        public static void SetSpeed(this IMotor device, int speed)
        {
            var body = new List<byte>();

            body.Add(device.Port);
            body.Add(0b00010000); // Startup and Completion Information 
            body.Add(0x51); // Write Direct
            body.Add(0x00); // Mode
            body.Add(0b10000001); // speed

            var bytes = new List<byte>();

            bytes.Add((byte)(body.Count() + 2)); // Length
            bytes.Add(0b00000000); // Hub ID
            bytes.Add(0x81); // Port Output Command
            bytes.AddRange(body);

            var message = new Message(bytes.ToArray());

            device.SendMessage(message);
        }

        public static void SetSpeedForDuration(this IMotor device, byte rotationalSpeed, byte maxPower, RotateDirection direction, short duration)
        {
            var angularVelocity = unchecked((byte)rotationalSpeed.AsAngularVelocity(direction));
            var time = BitConverter.GetBytes(Math.Min(Math.Max(duration, (short)0), (short)10000));
            var power = Math.Min(Math.Max(maxPower, (byte)0), (byte)100);

            var body = new List<byte>();
            body.Add(device.Port);
            body.Add(0b00010000);
            body.Add(0x09);
            body.AddRange(time); // time
            body.Add(angularVelocity); // speed + direction
            body.Add(power); // maximum power
            body.Add(0x0000); // end state
            body.Add(0x0000); // profile

            var bytes = new List<byte>();

            bytes.Add((byte)(body.Count() + 2)); // Length
            bytes.Add(0b00000000); // Hub ID
            bytes.Add(0x81); // Port Output Command
            bytes.AddRange(body);

            var message = new Message(bytes.ToArray());

            device.SendMessage(message);
        }

        public static void SetColor(this ILightEmittingDiode device, byte color)
        {
            var body = new List<byte>();

            body.Add(device.Port);
            body.Add(0b00010000); // Startup and Completion Information 
            body.Add(0x51); // Write Direct
            body.Add(0x00); // Mode
            body.Add(color); // Color

            var bytes = new List<byte>();

            bytes.Add((byte)(body.Count() + 2)); // Length
            bytes.Add(0b00000000); // Hub ID
            bytes.Add(0x81); // Port Output Command
            bytes.AddRange(body);

            var message = new Message(bytes.ToArray());

            device.SendMessage(message);
        }
    }
}
