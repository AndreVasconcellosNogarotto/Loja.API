namespace Loja.Application.DTOs
{
    public class SaleItemDto
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public ProductDto Product { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string Currency { get; set; } = "BRL";
        public decimal DiscountPercentage { get; set; }
        public decimal TotalPrice { get; set; }
        public bool Cancelled { get; set; }
    }
}
