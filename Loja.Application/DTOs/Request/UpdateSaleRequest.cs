namespace Loja.Application.DTOs.Request
{
    public class UpdateSaleRequest
    {
        public Guid SaleId { get; set; }
        public List<UpdateSaleItemRequest> Items { get; set; } = new List<UpdateSaleItemRequest>();
    }
}
