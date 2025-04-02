namespace Loja.Application.DTOs.Request
{
    public class ProductItemRequest
    {
        public string ProductExternalId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
