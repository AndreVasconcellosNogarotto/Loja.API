using Loja.Application.DTOs;
using Loja.Application.DTOs.Request;

namespace Loja.Application.Interfaces
{
    public interface ISaleService : IApplicationServiceBase<SaleDto, CreateSaleRequest, UpdateSaleRequest>
    {
        Task<SaleDto> GetSaleWithDetailsAsync(Guid id);
        Task<IEnumerable<SaleDto>> GetSalesByCustomerAsync(Guid customerId);
        Task<IEnumerable<SaleDto>> GetSalesByBranchAsync(Guid branchId);
        Task<IEnumerable<SaleDto>> GetSalesByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<bool> CancelSaleAsync(CancelSaleRequest request);
        Task<bool> CancelSaleItemAsync(CancelSaleItemRequest request);
    }
}
