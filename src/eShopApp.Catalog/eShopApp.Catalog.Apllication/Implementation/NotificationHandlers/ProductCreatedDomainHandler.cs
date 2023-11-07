using MediatR;
using eShopApp.Catalog.Domain.DomainEvents;

namespace eShopApp.Catalog.Apllication.Implementation.NotificationHandlers
{
    public sealed class ProductCreatedDomainHandler : INotificationHandler<ProductCreatedDomainEvent>
    {
        public Task Handle(ProductCreatedDomainEvent notification, CancellationToken cancellationToken)
        {
            // publish integration event

            return Task.CompletedTask;
        }
    }
}
