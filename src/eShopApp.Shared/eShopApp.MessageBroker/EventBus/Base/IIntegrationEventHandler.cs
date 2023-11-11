using eShopApp.MessageBroker.Models;

namespace eShopApp.MessageBroker.EventBus.Base
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IIntegrationEventHandler<T> where T : IntegrationEvent
    {
        Task Handle(T integrationEvent);
    }
}
