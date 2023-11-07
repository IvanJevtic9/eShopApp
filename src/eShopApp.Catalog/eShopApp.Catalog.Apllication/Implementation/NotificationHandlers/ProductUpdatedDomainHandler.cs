using MediatR;
using eShopApp.Catalog.Domain.DomainEvents;

namespace eShopApp.Catalog.Apllication.Implementation.NotificationHandlers
{
    public sealed class ProductUpdatedDomainHandler : INotificationHandler<ProductUpdatedDomainEvent>
    {
        public Task Handle(ProductUpdatedDomainEvent notification, CancellationToken cancellationToken)
        {
            // publish integration event

            return Task.CompletedTask;
        }
    }
}
