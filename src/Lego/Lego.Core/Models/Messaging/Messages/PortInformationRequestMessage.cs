namespace Lego.Core.Models.Messaging.Messages
{
    public class PortInformationRequestMessage : DownstreamMessage
    {
        public PortInformationRequestMessage(byte port, PortInformationType informationType) : base(MessageType.Port_Information_Request, new byte[] { port, (byte)informationType })
        {

        }
    }
}
