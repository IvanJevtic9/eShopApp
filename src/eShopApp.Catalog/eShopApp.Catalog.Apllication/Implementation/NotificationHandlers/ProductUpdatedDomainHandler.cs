using MediatR;
using eShopApp.Catalog.Domain.DomainEvents;
using eShopApp.Catalog.Contract.IntegrationEvents;
using eShopApp.Catalog.Domain.Entities;
using eShopApp.MessageBroker.EventBus.Base;
using eShopApp.Catalog.Infrastructure.DataAccess.Base;

namespace eShopApp.Catalog.Apllication.Implementation.NotificationHandlers
{
    public sealed class ProductUpdatedDomainHandler : INotificationHandler<ProductUpdatedDomainEvent>
    {
        private readonly IEventBus _eventBus;
        private readonly IUnitOfWork _unitOfWork;

        public ProductUpdatedDomainHandler(IEventBus eventBus, IUnitOfWork unitOfWork)
        {
            _eventBus = eventBus;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(ProductUpdatedDomainEvent notification, CancellationToken cancellationToken)
        {
            var product = await _unitOfWork.GetGenericRepository<Product>()
                .GetByIdAsync(notification.Id);

            var toPublish = false;

            string newName = null;
            if(product.Name != notification.OldName)
            {
                toPublish = true;
                newName = notification.OldName;
            }

            if(product.Price != notification.OldPrice)
            {
                toPublish = true;
            }

            if(toPublish)
            {
                _eventBus.Publish(new ProductUpdatedIntegrationEvent(
                    notification.Id,
                    notification.ProductId,
                    newName,
                    product.Price,
                    notification.OldPrice));
            }
        }
    }
}
