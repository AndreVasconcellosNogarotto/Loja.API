namespace Loja.Application.DTOs.Request
{
    public class CreateSaleRequest
    {
        public string CustomerExternalId { get; set; }
        public string BranchExternalId { get; set; }
        public List<ProductItemRequest> Items { get; set; } = new List<ProductItemRequest>();
    }
}
