using Polly;
using MediatR;
using Newtonsoft.Json;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using eShopApp.Catalog.Domain.Entities;
using eShopApp.Shared.DDAbstraction.Base;
using eShopApp.Catalog.Infrastructure.DataAccess.Base;

namespace eShopApp.Catalog.Apllication.HostedServices
{
    // FIX service descovery
    public class OutboxProcessorService : BackgroundService
    {
        private readonly IMediator _mediator;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<OutboxProcessorService> _logger;

        public OutboxProcessorService(
            IMediator mediator,
            IUnitOfWork unitOfWork,
            ILogger<OutboxProcessorService> logger)
        {
            _mediator = mediator;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Outbox Processor Service running.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var outboxRepository = _unitOfWork.GetGenericRepository<OutboxMessage>();
                    var outboxMessages = await outboxRepository
                        .GetAllAsync(p => !p.ProcessedOnUtc.HasValue);

                    var processPolicy = Policy
                        .Handle<Exception>()
                        .WaitAndRetryAsync(3, attempt => TimeSpan.FromMicroseconds(50 * attempt));

                    foreach (var message in outboxMessages)
                    {
                        var domainEvent = JsonConvert.DeserializeObject<IDomainEvent>(message.Content);
                        if(domainEvent is null)
                        {
                            _logger.LogError($"Failed to deserialize content to domain event model, message Id {message.Id}");
                            continue;
                        }

                        var result = await processPolicy.ExecuteAndCaptureAsync(async () => await _mediator.Publish(domainEvent, stoppingToken));

                        if (result.Outcome == OutcomeType.Successful)
                        {
                            message.ProcessedOnUtc = DateTime.UtcNow;
                            message.Error = null;
                        }
                        else
                        {
                            message.Error = result.FinalException?.Message;
                        }
                    }

                    await _unitOfWork.SaveChangesAsync();
                    await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occured while processing the outbox messages.");
                    await Task.Delay(TimeSpan.FromSeconds(60), stoppingToken);
                }
            }

            _logger.LogInformation("Outbox Processor Service stopping.");
        }
    }
}
