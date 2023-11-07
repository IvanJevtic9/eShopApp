using Newtonsoft.Json;
using eShopApp.Shared.DDAbstraction;
using eShopApp.Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace eShopApp.Catalog.Infrastructure.Interceptors
{
    public sealed class ConvertDomainEventsToOutboxMessagesInterceptors : SaveChangesInterceptor
    {
        public override async ValueTask<int> SavedChangesAsync(
            SaveChangesCompletedEventData eventData,
            int result,
            CancellationToken cancellationToken = default)
        {
            var dbContext = eventData.Context;

            if (dbContext is null)
            {
                return await base.SavedChangesAsync(eventData, result, cancellationToken);
            }

            var events = dbContext.ChangeTracker
                .Entries<AggregateRoot>()
                .Select(x => x.Entity)
                .SelectMany(aggregateRoot =>
                {
                    var domainEvents = aggregateRoot.GetDomainEvents();
                    aggregateRoot.ClearDomainEvents();

                    return domainEvents;
                })
                .Select(domainEvent => new OutboxMessage(domainEvent.Id)
                {
                    OccuredOnUtc = DateTime.Now,
                    Type = domainEvent.GetType().FullName,
                    Content = JsonConvert.SerializeObject(
                        domainEvent,
                        new JsonSerializerSettings
                        {
                            TypeNameHandling = TypeNameHandling.All
                        })
                })
                .ToList();

            if (events.Any())
            {
                await dbContext.Set<OutboxMessage>()
                    .AddRangeAsync(events);

                await dbContext.SaveChangesAsync();
            }

            return await base.SavedChangesAsync(eventData, result, cancellationToken);
        }
    }
}
