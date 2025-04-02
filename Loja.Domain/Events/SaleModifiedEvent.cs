using Loja.Domain.Entities;

namespace Loja.Domain.Events
{
    public class SaleModifiedEvent : DomainEvent
    {
        public Guid SaleId { get; }
        public string SaleNumber { get; }

        public SaleModifiedEvent(Sale sale)
        {
            SaleId = sale.Id;
            SaleNumber = sale.SaleNumber;
        }
    }
}
