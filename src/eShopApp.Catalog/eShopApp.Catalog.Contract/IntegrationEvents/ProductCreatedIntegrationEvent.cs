using eShopApp.MessageBroker.Models;

namespace eShopApp.Catalog.Contract.IntegrationEvents
{
    public sealed record ProductCreatedIntegrationEvent(
        Guid Id,
        Guid ProductId,
        string Name,
        decimal Price) 
    : IntegrationEvent(Id);
}
