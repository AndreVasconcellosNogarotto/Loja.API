using Loja.Domain.Entities;

namespace Loja.Domain.Events
{
    public class ItemCancelledEvent : DomainEvent
    {
        public Guid SaleId { get; }
        public string SaleNumber { get; }
        public Guid ItemId { get; }
        public string ProductName { get; }
        public string Reason { get; }

        public ItemCancelledEvent(Sale sale, SaleItem item, string reason = "")
        {
            SaleId = sale.Id;
            SaleNumber = sale.SaleNumber;
            ItemId = item.Id;
            ProductName = item.Product.Name;
            Reason = reason;
        }
    }
}
