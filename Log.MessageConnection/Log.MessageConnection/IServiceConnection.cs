using System;
using System.Threading.Tasks;

namespace Log.MessageConnection
{
    public interface IServiceConnection<T>
    {
        Task SendResponse(T content);
        void ReceiveRequest(Func<T, bool> callback);
    }
}
