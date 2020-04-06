using System;
using System.Threading.Tasks;

namespace Lego.Core
{
    public interface IConnection
    {
        Task Connect(Hub hub);
        void SendMessage(IMessage message);
    }

    public interface IConnectionManager
    {
        Task<T> EstablishHubConnectionById<T>(string deviceId) where T : Hub, new();
    }
}
