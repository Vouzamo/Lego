using System;
using System.Threading.Tasks;

namespace Lego.Core
{
    public interface IConnection
    {
        Task<T> EstablishHubConnectionById<T>(string deviceId) where T : Hub;

        event EventHandler<IMessage> OnMessageReceived;

        void SendMessage(string deviceId, IMessage message);
    }
}
