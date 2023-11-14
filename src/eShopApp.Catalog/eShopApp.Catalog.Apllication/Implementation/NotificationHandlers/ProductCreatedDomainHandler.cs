using MediatR;
using eShopApp.Catalog.Domain.DomainEvents;
using eShopApp.MessageBroker.EventBus.Base;
using eShopApp.MessageBroker.Models;
using eShopApp.Catalog.Contract.IntegrationEvents;
using eShopApp.Catalog.Infrastructure.DataAccess.Base;
using eShopApp.Catalog.Domain.Entities;

namespace eShopApp.Catalog.Apllication.Implementation.NotificationHandlers
{
    public sealed class ProductCreatedDomainHandler : INotificationHandler<ProductCreatedDomainEvent>
    {
        private readonly IEventBus _eventBus;
        private readonly IUnitOfWork _unitOfWork;

        public ProductCreatedDomainHandler(IEventBus eventBus, IUnitOfWork unitOfWork)
        {
            _eventBus = eventBus;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(ProductCreatedDomainEvent notification, CancellationToken cancellationToken)
        {
            var product = await _unitOfWork.GetGenericRepository<Product>()
                .GetByIdAsync(notification.Id);


            _eventBus.Publish(new ProductCreatedIntegrationEvent(
                notification.Id,
                notification.ProductId,
                product.Name,
                product.Price));
        }
    }
}
