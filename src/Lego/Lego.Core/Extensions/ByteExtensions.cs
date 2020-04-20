using Lego.Core.Models.Messaging;
using System;
using System.Collections;
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

        public static byte AsAngularVelocity(this byte speed, RotateDirection direction)
        {
            var rotationalSpeed = Math.Min(Math.Max(speed, (byte)0), (byte)100);
            sbyte angularVelocity = Convert.ToSByte(rotationalSpeed * (sbyte)direction);

            return unchecked((byte)angularVelocity);
        }

        public static IEnumerable<byte> ToModes(this ModeCombinations combination)
        {
            var modes = new List<byte>();

            var bits = new BitArray(BitConverter.GetBytes((ushort)combination));

            for (byte i = 0; i < bits.Length; i++)
            {
                if(bits[i])
                {
                    modes.Add(i);
                }
            }

            return modes;
        }
    }
}
