namespace Lego.Core.Models.Messaging
{
    public enum MessageType : byte
    {
        Hub__Properties = 0x01,
        Hub__Actions = 0x02,
        Hub__Alerts = 0x03,
        Hub__Attached_IO = 0x04,
        Generic_Error_Messages = 0x05,
        Hardware_Network_Commands = 0x08,
        Firmware_Update__Go_Into_Boot_Mode = 0x10,
        Firmware_Update__Lock_Memory = 0x11,
        Firmware_Update__Lock_Status_Request = 0x12,
        Firmware_Lock_Status = 0x13,
        Port_Information_Request = 0x21,
        Port_Mode_Information_Request = 0x22,
        Port_Input_Format_Setup__Single = 0x41,
        Port_Input_Format_Setup__Combined_Mode = 0x42,
        Port_Information = 0x43,
        Port_Mode_Information = 0x44,
        Port_Value__Single = 0x45,
        Port_Value__Combined_Mode = 0x46,
        Port_Input_Format__Single = 0x47,
        Port_Input_Format__Combined_Mode = 0x48,
        Virtual_Port_Setup = 0x61,
        Port_Output_Command = 0x81,
        Port_Output_Command_Feedback = 0x82
    }
}
