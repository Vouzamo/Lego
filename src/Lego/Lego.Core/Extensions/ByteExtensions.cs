using System.Collections.Generic;
using System.Linq;

namespace Lego.Core.Extensions
{
    public static class ByteExtensions
    {
        public static IEnumerable<byte> GetRange(this IEnumerable<byte> bytes, int skip, int take)
        {
            return bytes.Skip(skip).Take(take).ToList();
        }
    }

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
