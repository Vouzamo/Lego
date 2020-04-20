namespace Lego.Core.Models.Messaging
{
    public enum PortInputFormatSetupSubCommands : byte
    {
        Set_Mode_And_DataSet_Combinations = 1,
        Lock_LPF2_Device_For_Setup = 2,
        Unlock_And_Start_With_Multi_Update_Enabled = 3,
        Unlock_And_Start_With_Multi_Update_Disabled = 4,
        Reset_Sensor = 6
    }
}
