namespace Loja.Domain.Events
{
    public abstract class DomainEvent
    {
        public Guid Id { get; }
        public DateTime Timestamp { get; }

        protected DomainEvent()
        {
            Id = Guid.NewGuid();
            Timestamp = DateTime.UtcNow;
        }
    }
}
