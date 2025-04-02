using Loja.Domain.Entities;

namespace Loja.Domain.Interfaces
{
    public interface IBranchRepository : IRepositoryBase<Branch>
    {
        Task<Branch> GetByExternalIdAsync(string externalId);
    }
}
