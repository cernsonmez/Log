using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using System;

namespace Log.MessageConnection
{
    public class MessageQueueConnection : IMessageQueueConnection
    {

        public void CreateQueueNamespace(string queueName, string connectionString)
        {
            NamespaceManager namespaceManager = NamespaceManager.CreateFromConnectionString(connectionString);
            if (!namespaceManager.QueueExists(queueName))
            {
                QueueDescription qDes = new QueueDescription(queueName)
                {
                    MaxDeliveryCount = 1000,
                    AutoDeleteOnIdle = new TimeSpan(0, 10, 0),
                    EnableBatchedOperations = true,
                    SupportOrdering = true,
                    DefaultMessageTimeToLive = new TimeSpan(0, 5, 0) // Messages lives for 5 minutes
                };
                namespaceManager.CreateQueue(qDes);
                Console.WriteLine("queue oluştu");
            }
        }

        public void CreateTopicNamespace(string topicName, string connectionString, string channelName)
        {
            NamespaceManager namespaceManager = NamespaceManager.CreateFromConnectionString(connectionString);
            if (!namespaceManager.TopicExists(topicName))
            {
                namespaceManager.CreateTopic(topicName);
                SubscriptionDescription desc = new SubscriptionDescription(topicName, channelName)
                {
                    MaxDeliveryCount = 1000,
                    AutoDeleteOnIdle = new TimeSpan(0, 10, 0),
                    EnableBatchedOperations = true,
                    DefaultMessageTimeToLive = new TimeSpan(0, 5, 0) // Messages lives for 5 minutes
                };
                namespaceManager.CreateSubscription(desc);
                Console.WriteLine("topic oluştu");
            }
        }
    }
}

