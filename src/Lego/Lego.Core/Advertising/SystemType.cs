using System;
using System.Collections.Generic;
using System.Text;

namespace Lego.Core.Advertising
{
    public enum SystemType
    {
        LEGO_Wedo_2 = 0b000,
        LEGO_Duplo = 0b001,
        LEGO_System_A = 0b010,
        LEGO_System_B = 0b011
    }

    public enum DeviceNumber
    {
        WeDo_Hub = 0b00000000,
        Duplo_Train = 0b00100000,
        Boost_Hub = 0b01000000,
        Two_Port_Hub = 0b01000001,
        Two_Port_Handset = 0b01000010
    }



    public enum Channel : byte
    {
        Channel1 = 0x0,
        Channel2 = 0x1,
        Channel3 = 0x2,
        Channel4 = 0x3
    }

    public enum HubConnector : byte
    {
        A = 0x0,
        B = 0x1,
        C = 0x2,
        D = 0x3
    }
}
