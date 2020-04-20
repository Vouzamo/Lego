using System;
using System.Collections.Generic;

namespace Lego.Core.Models.Messaging.Messages
{
    public class PortInputFormatSetupSingleMessage : DownstreamMessage
    {
        public PortInputFormatSetupSingleMessage(byte port, byte mode, uint delta, bool notify) : base(MessageType.Port_Input_Format_Setup__Single, ToByteArray(port, mode, delta, notify))
        {

        }

        public static byte[] ToByteArray(byte port, byte mode, uint delta, bool notify)
        {
            var bytes = new List<byte>
            {
                port,
                mode
            };

            bytes.AddRange(BitConverter.GetBytes(delta));
            bytes.AddRange(BitConverter.GetBytes(notify));

            return bytes.ToArray();
        }
    }
}
