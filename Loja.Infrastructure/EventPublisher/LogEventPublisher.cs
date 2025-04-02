using Microsoft.Extensions.Logging;
using Loja.Domain.Events;

namespace Loja.Infrastructure.EventPublisher
{
    public class LogEventPublisher : IEventPublisher
    {
        private readonly ILogger<LogEventPublisher> _logger;

        public LogEventPublisher(ILogger<LogEventPublisher> logger)
        {
            _logger = logger;
        }

        public Task PublishAsync<T>(T @event) where T : DomainEvent
        {
            _logger.LogInformation($"Event published: {typeof(T).Name} - {@event.Id}");

            switch (@event)
            {
                case SaleCreatedEvent saleCreated:
                    _logger.LogInformation($"Sale {saleCreated.SaleNumber} created by {saleCreated.CustomerName} at {saleCreated.BranchName}");
                    break;
                case SaleModifiedEvent saleModified:
                    _logger.LogInformation($"Sale {saleModified.SaleNumber} modified");
                    break;
                case SaleCancelledEvent saleCancelled:
                    _logger.LogInformation($"Sale {saleCancelled.SaleNumber} cancelled. Reason: {saleCancelled.Reason}");
                    break;
                case ItemCancelledEvent itemCancelled:
                    _logger.LogInformation($"Item {itemCancelled.ItemId} ({itemCancelled.ProductName}) from sale {itemCancelled.SaleNumber} cancelled. Reason: {itemCancelled.Reason}");
                    break;
            }

            return Task.CompletedTask;
        }
    }
}
