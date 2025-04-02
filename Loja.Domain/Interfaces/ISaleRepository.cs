using Loja.Domain.Entities;

namespace Loja.Domain.Interfaces
{
    public interface ISaleRepository : IRepositoryBase<Sale>
    {
        Task<Sale> GetSaleWithDetailsAsync(Guid id);
        Task<IEnumerable<Sale>> GetSalesByCustomerAsync(Guid customerId);
        Task<IEnumerable<Sale>> GetSalesByBranchAsync(Guid branchId);
        Task<IEnumerable<Sale>> GetSalesByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<string> GenerateSaleNumberAsync();
    }
}
