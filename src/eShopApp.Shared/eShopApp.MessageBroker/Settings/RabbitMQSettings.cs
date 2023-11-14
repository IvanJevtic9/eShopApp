namespace eShopApp.MessageBroker.Settings
{
    public class RabbitMQSettings
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public int RetryCount { get; set; }
        public string SubscriptionClientName { get; set; }
    }
}
