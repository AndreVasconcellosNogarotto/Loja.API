namespace Loja.Application.DTOs.Request
{
    public class CancelSaleItemRequest
    {
        public Guid SaleId { get; set; }
        public Guid ItemId { get; set; }
        public string Reason { get; set; }
    }
}
