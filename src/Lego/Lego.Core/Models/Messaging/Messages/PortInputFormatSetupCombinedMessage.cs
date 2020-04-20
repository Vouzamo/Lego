using System.Collections.Generic;

namespace Lego.Core.Models.Messaging.Messages
{
    public class PortInputFormatSetupCombinedMessage : DownstreamMessage
    {
        public PortInputFormatSetupCombinedMessage(byte port, PortInputFormatSetupSubCommands command) : base(MessageType.Port_Input_Format_Setup__Combined_Mode, new byte[] { port, (byte)command })
        {

        }
    }

    public class PortInputFormatSetupCombinedSetModeDatasetMessage : DownstreamMessage
    {
        public PortInputFormatSetupCombinedSetModeDatasetMessage(byte port, byte combinationIndex, IEnumerable<byte> modeAndDatasetCombinations) : base(MessageType.Port_Input_Format_Setup__Combined_Mode, ToBytes(port, combinationIndex, modeAndDatasetCombinations))
        {

        }

        public static byte[] ToBytes(byte port, byte combinationIndex, IEnumerable<byte> modeAndDatasetCombinations)
        {
            var bytes = new List<byte>
            {
                port,
                (byte)PortInputFormatSetupSubCommands.Set_Mode_And_DataSet_Combinations,
                combinationIndex
            };

            bytes.AddRange(modeAndDatasetCombinations);

            return bytes.ToArray();
        }
    }
}
