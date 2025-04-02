using Loja.Domain.ValueObject;

namespace Loja.Domain.Entities
{
    public class SaleItem : EntityBase
    {
        public Guid SaleId { get; private set; }
        public Guid ProductId { get; private set; }
        public Product Product { get; private set; }
        public int Quantity { get; private set; }
        public Money UnitPrice { get; private set; }
        public decimal DiscountPercentage { get; private set; }
        public bool Cancelled { get; private set; }

        public Money TotalPrice =>
            !Cancelled ?
            UnitPrice * Quantity * (1 - DiscountPercentage / 100) :
            new Money(0);

        protected SaleItem() { }

        public SaleItem(Guid saleId, Product product, int quantity, Money unitPrice)
        {
            if (saleId == Guid.Empty)
                throw new ArgumentException("SaleId cannot be empty", nameof(saleId));

            if (product == null)
                throw new ArgumentNullException(nameof(product));

            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero", nameof(quantity));

            if (quantity > 20)
                throw new ArgumentException("Quantity cannot exceed 20 items", nameof(quantity));

            if (unitPrice.Value <= 0)
                throw new ArgumentException("Unit price must be greater than zero", nameof(unitPrice));

            SaleId = saleId;
            Product = product;
            ProductId = product.Id;
            Quantity = quantity;
            UnitPrice = unitPrice;

            ApplyDiscountRules();
        }

        private void ApplyDiscountRules()
        {
            if (Quantity >= 10 && Quantity <= 20)
            {
                DiscountPercentage = 20;
            }
            else if (Quantity >= 4)
            {
                DiscountPercentage = 10;
            }
            else
            {
                DiscountPercentage = 0;
            }
        }

        public void UpdateQuantity(int quantity)
        {
            if (Cancelled)
                throw new InvalidOperationException("Cannot update a cancelled item");

            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero", nameof(quantity));

            if (quantity > 20)
                throw new ArgumentException("Quantity cannot exceed 20 items", nameof(quantity));

            Quantity = quantity;

            ApplyDiscountRules();
            SetUpdatedAt();
        }

        public void Cancel()
        {
            if (Cancelled)
                return;

            Cancelled = true;
            SetUpdatedAt();
        }
    }
}
