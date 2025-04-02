using Loja.Domain.Events;

namespace Loja.Infrastructure.EventPublisher
{
    public interface IEventPublisher
    {
        Task PublishAsync<T>(T @event) where T : DomainEvent;
    }
}
