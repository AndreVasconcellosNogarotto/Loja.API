namespace Loja.Application.DTOs
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        public string ExternalId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal BasePrice { get; set; }
        public string Currency { get; set; } = "BRL";
    }
}
