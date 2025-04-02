using Loja.Domain.Entities;

namespace Loja.Domain.Events
{
    public class SaleCancelledEvent : DomainEvent
    {
        public Guid SaleId { get; }
        public string SaleNumber { get; }
        public string Reason { get; }

        public SaleCancelledEvent(Sale sale, string reason = "")
        {
            SaleId = sale.Id;
            SaleNumber = sale.SaleNumber;
            Reason = reason;
        }
    }

}
