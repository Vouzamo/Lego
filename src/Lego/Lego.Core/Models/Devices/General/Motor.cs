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

        public int MinPosition { get; protected set; } = int.MinValue;
        public int MaxPosition { get; protected set; } = int.MaxValue;
        public int MidPosition => (MinPosition + MaxPosition) / 2;

        public int Position { get; protected set; }
        public byte Speed { get; protected set; }

        public Motor(Hub hub, byte port) : base(hub, port)
        {
            //SendMessage(new PortInputFormatSetupMessage(Port, INPUT_MODE__POSITION, 1, true));
            //SendMessage(new PortInputFormatSetupMessage(Port, INPUT_MODE__SPEED, 1, true));
        }

        public override void HandleValue(byte[] bytes)
        {
            if (InputMode == INPUT_MODE__POSITION)
            {
                Position = BitConverter.ToInt32(bytes, 1);
            }
            else if (InputMode == INPUT_MODE__SPEED)
            {
                Speed = bytes.ElementAt(1);
            }
        }

        public async Task AutoCalibrate(byte power)
        {
            this.SetInputModes(new byte[] { Motor.INPUT_MODE__SPEED });

            await Task.Delay(1000);

            this.GotoAbsolutePositionMin(100, power);

            do
            {
                await Task.Delay(1000);
            }
            while (this.Speed > 0);

            this.SetInputModes(new[] { Motor.INPUT_MODE__POSITION });

            await Task.Delay(1000);

            int? minPosition = Position;

            this.SetInputModes(new[] { Motor.INPUT_MODE__SPEED });

            await Task.Delay(1000);

            this.GotoAbsolutePositionMax(100, power);

            do
            {
                await Task.Delay(1000);
            }
            while (this.Speed > 0);

            this.SetInputModes(new[] { Motor.INPUT_MODE__POSITION });

            await Task.Delay(1000);

            int? maxPosition = Position;
            MinPosition = minPosition.Value;
            MaxPosition = maxPosition.Value;

            this.GotoAbsolutePositionMid(100, 100);
        }
    }
}
