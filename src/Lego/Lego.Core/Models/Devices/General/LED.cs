namespace Lego.Core.Models.Devices.General
{
    public class LED : Device, ILightEmittingDiode
    {
        public LED(Hub hub, byte port) : base(hub, port)
        {

        }
    }
}
