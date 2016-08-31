using System;
using System.Threading.Tasks;

namespace Log.MessageConnection
{
    public interface IConsumerConnection<T>
    {
        Task SendRequest(T content);
        void ReceiveResponse(Func<T, bool> callback);
    }
}
