using System;
using System.Threading.Tasks;

namespace Lego.Core
{
    public interface IConnection
    {
        Task Connect(Hub hub);
        void SendMessage(IMessage message);
    }
}
