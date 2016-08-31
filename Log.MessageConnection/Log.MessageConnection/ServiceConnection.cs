using Microsoft.ServiceBus.Messaging;
using System;
using System.Threading.Tasks;

namespace Log.MessageConnection
{
    public class ServiceConnection<T> : IServiceConnection<T>
    {
        delegate bool UserCommandHandler(T info);
        private event UserCommandHandler _userCommand;

        private QueueClient _requestClient;
        private TopicClient _topicClient;

        public ServiceConnection(string connString, string queueName, string topicName)
        {
            MessageQueueConnection messageQueueConnection = new MessageQueueConnection();
            messageQueueConnection.CreateQueueNamespace(queueName, connString);
            //messageQueueConnection.CreateTopicNamespace(queueName, connString, Guid.NewGuid().ToString());

            MessagingFactory messagingFactory = null;
            messagingFactory = MessagingFactory.CreateFromConnectionString(connString);

            _requestClient = messagingFactory.CreateQueueClient(queueName, ReceiveMode.PeekLock);
            _topicClient = messagingFactory.CreateTopicClient(topicName);

        }

        public async Task SendResponse(T content)
        {
            try
            {
                var brokeredMessage = new BrokeredMessage(content);
                await _topicClient.SendAsync(brokeredMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine("hata " + ex.Message);
            }

            return;
        }
        OnMessageOptions options = new OnMessageOptions
        {
            MaxConcurrentCalls = 5,
            AutoComplete = false
        };

        private Task ownReceiver(BrokeredMessage message)
        {
            //successState; program çalışırken catch'e düşmesi durumunda message.AbandonAsync() döndürmesi gerektiğinden false olarak tanımlanmıştır.
            bool successState = false;
            try
            {
                Console.WriteLine("DONE");
                successState = _userCommand(message.GetBody<T>());

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            if (successState)
            {
                return message.CompleteAsync();
            }
            else
            {
                return message.AbandonAsync();
            }
        }

        public void ReceiveRequest(Func<T, bool> callback)
        {

            _userCommand += new UserCommandHandler(callback);
            _requestClient.OnMessageAsync(ownReceiver, options);
        }

    }
}

