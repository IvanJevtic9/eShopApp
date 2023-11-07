using eShopApp.Shared.DDAbstraction;

namespace eShopApp.Catalog.Domain.DomainEvents
{
    public sealed record ProductCreatedDomainEvent(Guid ProductId) : DomainEvent(Guid.NewGuid());
}
