namespace Log.MessageConnection
{
    public interface IMessageQueueConnection
    {
        void CreateQueueNamespace(string queueName, string connectionString);
        void CreateTopicNamespace(string topicName, string connectionString, string channelName);
    }
}
