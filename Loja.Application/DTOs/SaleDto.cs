namespace Loja.Application.DTOs
{
    public class SaleDto
    {
        public Guid Id { get; set; }
        public string SaleNumber { get; set; }
        public DateTime SaleDate { get; set; }
        public Guid CustomerId { get; set; }
        public CustomerDto Customer { get; set; }
        public Guid BranchId { get; set; }
        public BranchDto Branch { get; set; }
        public decimal TotalAmount { get; set; }
        public string Currency { get; set; } = "BRL";
        public bool Cancelled { get; set; }
        public List<SaleItemDto> Items { get; set; } = new List<SaleItemDto>();
    }
}
