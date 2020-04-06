using System.Collections.Generic;

namespace Lego.Core.Models.Messaging
{
    public class VersionNumber
    {
        public byte Major { get; }
        public byte Minor { get; }
        public byte Patch { get; }
        public ushort Build { get; }

        protected IEnumerable<byte> Bytes { get; }

        public VersionNumber(IEnumerable<byte> bytes)
        {
            Bytes = bytes;
        }
    }
}
