using Lego.Core.Extensions;
using Lego.Core.Models.Messaging;
using Lego.Core.Models.Messaging.Messages;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Lego.Core.Models.Devices.General
{
    public class Motor : Device, IMotor
    {
        public const byte INPUT_MODE__SPEED = 0x01;
        public const byte INPUT_MODE__POSITION = 0x02;

        public int MinPosition { get; set; } = int.MinValue;
        public int MaxPosition { get; set; } = int.MaxValue;
        public int MidPosition => (MinPosition + MaxPosition) / 2;

        public int Position { get; set; }
        public byte Speed { get; set; }

        public Motor(Hub hub, byte port) : base(hub, port)
        {
            
        }

        public override void HandleValue(byte[] bytes)
        {
            var modes = InputMode.ToModes();

            if(modes.Any())
            {
                if(modes.Count() > 1)
                {
                    // Combined Mode

                    Speed = bytes.ElementAt(1);
                    Position = BitConverter.ToInt32(bytes, 2);
                }
                else
                {
                    // Single Mode

                    if (modes.First() == INPUT_MODE__SPEED)
                    {
                        Speed = bytes.ElementAt(1);
                    }
                    else if (modes.First() == INPUT_MODE__POSITION)
                    {
                        Position = BitConverter.ToInt32(bytes, 1);
                    }
                }
            }
        }
    }
}
