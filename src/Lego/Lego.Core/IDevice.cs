using Lego.Core.Models.Messaging.Messages;
using System.Threading.Tasks;

namespace Lego.Core
{
    public interface IDevice
    {
        Hub Hub { get; }
        byte Port { get; }

        bool IsReady { get; }

        void SetSingleInputMode(byte mode, uint delta = 1, bool notify = true);
        Task<bool> SetCombinedInputMode(byte combinationModeIndex, uint delta = 1, bool notify = true);
        void SendMessage(IMessage message);
        void ReceiveMessage(IMessage message);
    }
}
