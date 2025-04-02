using Loja.Domain.Entities;

namespace Loja.Domain.Events
{
    public class SaleCreatedEvent : DomainEvent
    {
        public Guid SaleId { get; }
        public string SaleNumber { get; }
        public DateTime SaleDate { get; }
        public string CustomerName { get; }
        public string BranchName { get; }

        public SaleCreatedEvent(Sale sale)
        {
            SaleId = sale.Id;
            SaleNumber = sale.SaleNumber;
            SaleDate = sale.SaleDate;
            CustomerName = sale.Customer.Name;
            BranchName = sale.Branch.Name;
        }
    }
}
