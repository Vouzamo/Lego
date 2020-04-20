namespace Lego.Core.Models.Messaging.Messages
{
    public class PortModeInformationRequestMessage : DownstreamMessage
    {
        public PortModeInformationRequestMessage(byte port, byte mode, PortModeInformationType informationType) : base(MessageType.Port_Mode_Information_Request, new byte[] { port, mode, (byte)informationType })
        {

        }
    }
}
