using MediatR;

namespace eShopApp.Shared.DDAbstraction.Base
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDomainEvent : INotification
    {
        Guid Id { get; }
    }
}
