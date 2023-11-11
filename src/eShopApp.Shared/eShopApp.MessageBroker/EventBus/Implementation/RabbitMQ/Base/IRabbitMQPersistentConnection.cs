using RabbitMQ.Client;

namespace eShopApp.MessageBroker.EventBus.Implementation.RabbitMQ.Base
{
    /// <summary>
    /// 
    /// </summary>
    public interface IRabbitMQPersistentConnection : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        bool IsConnected { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        bool TryConnect();
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IModel CreateModel();
    }
}
