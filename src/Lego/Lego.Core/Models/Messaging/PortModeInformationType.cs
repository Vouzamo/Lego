namespace Lego.Core.Models.Messaging
{
    public enum PortModeInformationType : byte
    {
        Name = 0x00,
        Raw_Range = 0x01,
        Percent_Range = 0x02,
        SI_Value_Range = 0x03,
        Symbol = 0x04,
        Mapping = 0x05,
        Motor_Bias = 0x07,
        Capability_Bits = 0x08,
        ValueFormat = 0x80
    }
}
