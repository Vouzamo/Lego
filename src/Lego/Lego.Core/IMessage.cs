using Lego.Core.Models.Messaging;
using System.Collections.Generic;

namespace Lego.Core
{
    public interface IMessage
    {
        IEnumerable<byte> Bytes { get; }
        IEnumerable<byte> CommonHeader { get; }
        IEnumerable<byte> Body { get; }

        ushort MessageLength { get; }
        byte HubID { get; }
        MessageType MessageType { get; }
    }
}
