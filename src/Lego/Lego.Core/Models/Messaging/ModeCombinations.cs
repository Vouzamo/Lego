using System;

namespace Lego.Core.Models.Messaging
{
    [Flags]
    public enum ModeCombinations : ushort
    {
        Zero = 0b_0000_0000_0000_0001,
        One = 0b_0000_0000_0000_0010,
        Two = 0b_0000_0000_0000_0100,
        Three = 0b_0000_0000_0000_1000,
        Four = 0b_0000_0000_0001_0000,
        Five = 0b_0000_0000_0010_0000,
        Six = 0b_0000_0000_0100_0000,
        Seven = 0b_0000_0000_1000_0000,
        Eight = 0b_0000_0001_0000_0000,
        Nine = 0b_0000_0010_0000_0000,
        Ten = 0b_0000_0100_0000_0000,
        Eleven = 0b_0000_1000_0000_0000,
        Twelve = 0b_0001_0000_0000_0000,
        Thirteen = 0b_0010_0000_0000_0000,
        Fourteen = 0b_0100_0000_0000_0000,
        Fifteen = 0b_1000_0000_0000_0000,
    }
}
