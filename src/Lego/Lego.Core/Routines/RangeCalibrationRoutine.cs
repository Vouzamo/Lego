using Lego.Core.Extensions;
using Lego.Core.Models.Devices.General;
using System;
using System.Threading.Tasks;

namespace Lego.Core
{
    public class RangeCalibrationRoutine : BaseRoutine<Motor>
    {
        public override Func<Motor, bool> StartCondition => (device) => true; // start immediately
        public override Func<Motor, bool> StopCondition => (device) => IsCalibrated; // wait until calibration has completed

        protected bool IsCalibrated { get; set; } = false;
        protected byte Power { get; set; }

        public RangeCalibrationRoutine(byte power)
        {
            Power = power;
        }

        public async override void Routine(Motor device)
        {
            //device.SetSpeedForDuration(100, Power, RotateDirection.Clockwise, 500);
            //await Task.Delay(750);
            //device.SetSpeedForDuration(100, Power, RotateDirection.CounterClockwise, 250);
            //await Task.Delay(750);

            device.SetInputModes(new byte[] { Motor.INPUT_MODE__SPEED });

            await Task.Delay(1000);

            device.GotoAbsolutePositionMin(100, Power);

            do
            {
                await Task.Delay(1000);
            }
            while (device.Speed > 0);

            device.SetInputModes(new[] { Motor.INPUT_MODE__POSITION });

            await Task.Delay(1000);

            int? minPosition = device.Position;

            device.SetInputModes(new[] { Motor.INPUT_MODE__SPEED });

            await Task.Delay(1000);

            device.GotoAbsolutePositionMax(100, Power);

            do
            {
                await Task.Delay(1000);
            }
            while (device.Speed > 0);

            device.SetInputModes(new[] { Motor.INPUT_MODE__POSITION });

            await Task.Delay(1000);

            int? maxPosition = device.Position;

            device.MinPosition = minPosition.Value;
            device.MaxPosition = maxPosition.Value;

            device.GotoAbsolutePositionMid(100, 100);

            await Task.Delay(2000);

            IsCalibrated = true;
        }
    }
}
