using System;

namespace Lego.Core.Models.Messaging
{
    [Flags]
    public enum PortCapabilities : byte
    {
        Output = 0b0001,
        Input = 0b0010,
        Combinable = 0b0100,
        Synchronizable = 0b1000
    }
}
