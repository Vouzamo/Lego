namespace Lego.Core.Models.Devices.Parts
{

    public class TechnicMotorL : Device, IMotor
    {
        public TechnicMotorL(Hub hub, byte port) : base(hub, port)
        {

        }
    }
}
