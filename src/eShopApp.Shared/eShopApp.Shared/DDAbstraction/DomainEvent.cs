using eShopApp.Shared.DDAbstraction.Base;

namespace eShopApp.Shared.DDAbstraction
{
    public abstract record DomainEvent(Guid Id) : IDomainEvent;
}
