namespace eShopApp.MessageBroker.EventBus.Base
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDynamicIntegrationEventHandler
    {
        Task Handle(dynamic integrationEvent);
    }
}
