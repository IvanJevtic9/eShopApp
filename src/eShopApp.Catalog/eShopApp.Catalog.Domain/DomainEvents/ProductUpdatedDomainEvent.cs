using eShopApp.Shared.DDAbstraction;

namespace eShopApp.Catalog.Domain.DomainEvents
{
    public sealed record ProductUpdatedDomainEvent(
        Guid ProductId,
        string OldName,
        decimal OldPrice
    ) : DomainEvent(Guid.NewGuid());
}
