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
            throw new NotImplementedException("speed needs to be a byte and assigned to the body of the message.");

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

        public static void GotoAbsolutePositionMin(this IMotor device, byte speed, byte power)
        {
            GotoAbsolutePosition(device, device.MinPosition, speed, power);
        }

        public static void GotoAbsolutePositionMax(this IMotor device, byte speed, byte power)
        {
            GotoAbsolutePosition(device, device.MaxPosition, speed, power);
        }

        public static void GotoAbsolutePositionMid(this IMotor device, byte speed, byte power)
        {
            GotoAbsolutePosition(device, device.MidPosition, speed, power);
        }

        public static void GotoAbsolutePosition(this IMotor device, int position, byte speed, byte power)
        {
            position = Math.Min(Math.Max(position, device.MinPosition), device.MaxPosition);
            speed = Math.Min(Math.Max(speed, (byte)0), (byte)100);
            power = Math.Min(Math.Max(power, (byte)0), (byte)100);

            var body = new List<byte>();
            body.Add(device.Port);
            body.Add(0b00010000);
            body.Add(0x0D);
            body.AddRange(BitConverter.GetBytes(position));
            body.Add(speed);
            body.Add(power);
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

        public static void SetSpeedForDuration(this IMotor device, byte speed, byte power, RotateDirection direction, short duration)
        {
            speed = speed.AsAngularVelocity(direction);
            duration = Math.Min(Math.Max(duration, (short)0), (short)10000);
            power = Math.Min(Math.Max(power, (byte)0), (byte)100);

            var body = new List<byte>();
            body.Add(device.Port);
            body.Add(0b00010000);
            body.Add(0x09);
            body.AddRange(BitConverter.GetBytes(duration));
            body.Add(speed);
            body.Add(power);
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
