using eShopApp.MessageBroker.Models;

namespace eShopApp.Catalog.Contract.IntegrationEvents
{
    public sealed record ProductUpdatedIntegrationEvent(
        Guid Id,
        Guid ProductId,
        string NewName,
        decimal NewPrice,
        decimal OldPrice)
    : IntegrationEvent(Id);
}
