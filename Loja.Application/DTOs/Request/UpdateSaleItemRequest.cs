namespace Loja.Application.DTOs.Request
{
    public class UpdateSaleItemRequest
    {
        public Guid ItemId { get; set; }
        public int Quantity { get; set; }
    }
}
