namespace Lego.Core.Models.Messaging
{
    public enum IODeviceType : ushort
    {
        Unknown = 0x0000,
        Motor = 0x0001,
        System_Train_Motor = 0x0002,
        Button = 0x0005,
        LED_Light = 0x0008,
        Voltage = 0x0014,
        Current = 0x0015,
        Piezo_Tone_ = 0x0016,
        RGB_Light = 0x0017,
        External_Tilt_Sensor = 0x0022,
        Motion_Sensor = 0x0023,
        Vision_Sensor = 0x0025,
        External_Motor_With_Tacho = 0x0026,
        Internal_Motor_With_Tacho = 0x0027,
        Internal_Tilt = 0x0028,
        TechnicMotorL = 0x002E,
        TechnicMotorXL = 0x002F
    }
}
