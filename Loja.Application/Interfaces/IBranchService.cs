using Loja.Application.DTOs;

namespace Loja.Application.Interfaces
{
    public interface IBranchService : IApplicationServiceBase<BranchDto, BranchDto, BranchDto>
    {
        Task<BranchDto> GetByExternalIdAsync(string externalId);
    }
}
