using Loja.Domain.ValueObject;

namespace Loja.Domain.Entities
{
    public class Sale : EntityBase
    {
        public string SaleNumber { get; private set; }
        public DateTime SaleDate { get; private set; }
        public Guid CustomerId { get; private set; }
        public Customer Customer { get; private set; }
        public Guid BranchId { get; private set; }
        public Branch Branch { get; private set; }
        public bool Cancelled { get; private set; }

        private readonly List<SaleItem> _items = new List<SaleItem>();
        public IReadOnlyCollection<SaleItem> Items => _items.AsReadOnly();

        public Money TotalAmount =>
            !Cancelled ?
            new Money(_items.Where(i => !i.Cancelled).Sum(i => i.TotalPrice.Value)) :
            new Money(0);

        protected Sale() { }

        public Sale(string saleNumber, Customer customer, Branch branch)
        {
            if (string.IsNullOrWhiteSpace(saleNumber))
                throw new ArgumentException("Sale number cannot be empty", nameof(saleNumber));

            if (customer == null)
                throw new ArgumentNullException(nameof(customer));

            if (branch == null)
                throw new ArgumentNullException(nameof(branch));

            SaleNumber = saleNumber;
            SaleDate = DateTime.UtcNow;
            Customer = customer;
            CustomerId = customer.Id;
            Branch = branch;
            BranchId = branch.Id;
            Cancelled = false;
        }

        public SaleItem AddItem(Product product, int quantity, Money unitPrice)
        {
            if (Cancelled)
                throw new InvalidOperationException("Cannot add items to a cancelled sale");

            if (product == null)
                throw new ArgumentNullException(nameof(product));

            var existingItem = _items.FirstOrDefault(i => i.ProductId == product.Id && !i.Cancelled);
            if (existingItem != null)
            {
                throw new InvalidOperationException("This product is already in the sale. Update the existing item instead.");
            }

            var saleItem = new SaleItem(Id, product, quantity, unitPrice);
            _items.Add(saleItem);

            SetUpdatedAt();
            return saleItem;
        }

        public void UpdateItem(Guid itemId, int quantity)
        {
            if (Cancelled)
                throw new InvalidOperationException("Cannot update items in a cancelled sale");

            var item = _items.FirstOrDefault(i => i.Id == itemId);
            if (item == null)
                throw new ArgumentException($"Item with ID {itemId} not found in this sale");

            if (item.Cancelled)
                throw new InvalidOperationException("Cannot update a cancelled item");

            item.UpdateQuantity(quantity);
            SetUpdatedAt();
        }

        public void CancelItem(Guid itemId)
        {
            if (Cancelled)
                throw new InvalidOperationException("Cannot cancel items in a cancelled sale");

            var item = _items.FirstOrDefault(i => i.Id == itemId);
            if (item == null)
                throw new ArgumentException($"Item with ID {itemId} not found in this sale");

            if (item.Cancelled)
                return;

            item.Cancel();
            SetUpdatedAt();
        }

        public void Cancel()
        {
            if (Cancelled)
                return;

            Cancelled = true;

            // Cancelar todos os itens
            foreach (var item in _items.Where(i => !i.Cancelled))
            {
                item.Cancel();
            }

            SetUpdatedAt();
        }
    }
}
