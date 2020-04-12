namespace Lego.Core.Models.Devices
{
    public interface IMotor : IDevice
    {
        int MinPosition { get; }
        int MaxPosition { get; }
        int MidPosition { get; }
    }
}
