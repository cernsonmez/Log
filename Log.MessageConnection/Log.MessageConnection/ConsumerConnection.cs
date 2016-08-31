using Microsoft.ServiceBus.Messaging;
using System;
using System.Threading.Tasks;

namespace Log.MessageConnection
{
    /// <summary>
    /// Consumer ile Servis Bus arasındaki bağlantı
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ConsumerConnection<T> : IConsumerConnection<T>
    {
        delegate bool UserCommandHandler(T info);
        private event UserCommandHandler _userCommand;

        private QueueClient _requestClient;
        private TopicClient _topicClient;

        public ConsumerConnection(string connString, string queueName, string topicName)
        {
            MessageQueueConnection messageQueueConnection = new MessageQueueConnection();
            messageQueueConnection.CreateQueueNamespace(queueName, connString);
            messageQueueConnection.CreateTopicNamespace(topicName, connString, Guid.NewGuid().ToString());

            MessagingFactory messagingFactory = null;
            messagingFactory = MessagingFactory.CreateFromConnectionString(connString);

            _requestClient = messagingFactory.CreateQueueClient(queueName, ReceiveMode.PeekLock);
            _topicClient = messagingFactory.CreateTopicClient(topicName);

        }
        /// <summary>
        /// Verileri queueya kaydeder.
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public async Task SendRequest(T content)
        {
            try
            {
                var brokeredMessage = new BrokeredMessage(content);
                await _requestClient.SendAsync(brokeredMessage);
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

        public void ReceiveResponse(Func<T, bool> callback)
        {

            _userCommand += new UserCommandHandler(callback);
            Console.WriteLine("Kayıt yapılmalı");
            //_requestClient.OnMessageAsync(ownReceiver, options);

        }

    }
}

