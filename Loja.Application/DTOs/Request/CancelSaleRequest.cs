namespace Loja.Application.DTOs.Request
{
    public class CancelSaleRequest
    {
        public Guid SaleId { get; set; }
        public string Reason { get; set; }
    }
}
