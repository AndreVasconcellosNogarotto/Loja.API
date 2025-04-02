using Loja.Domain.ValueObject;

namespace Loja.Domain.Entities
{
    public class Product : EntityBase
    {
        public string ExternalId { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public Money BasePrice { get; private set; }

        // Para o ORM
        protected Product() { }

        public Product(string externalId, string name, string description, Money basePrice)
        {
            if (string.IsNullOrWhiteSpace(externalId))
                throw new ArgumentException("ExternalId cannot be empty", nameof(externalId));

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be empty", nameof(name));

            if (basePrice.Value < 0)
                throw new ArgumentException("Base price cannot be negative", nameof(basePrice));

            ExternalId = externalId;
            Name = name;
            Description = description;
            BasePrice = basePrice;
        }

        public void Update(string name, string description, Money basePrice)
        {
            if (!string.IsNullOrWhiteSpace(name))
                Name = name;

            if (!string.IsNullOrWhiteSpace(description))
                Description = description;

            if (basePrice.Value >= 0)
                BasePrice = basePrice;

            SetUpdatedAt();
        }
    }
}
