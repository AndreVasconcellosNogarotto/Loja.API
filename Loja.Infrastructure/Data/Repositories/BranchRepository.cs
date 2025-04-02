using Microsoft.EntityFrameworkCore;
using Loja.Domain.Entities;
using Loja.Domain.Interfaces;
using Loja.Infrastructure.Repositories;

namespace Loja.Infrastructure.Data.Repositories
{
    public class BranchRepository : RepositoryBase<Branch>, IBranchRepository
    {
        public BranchRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Branch> GetByExternalIdAsync(string externalId)
        {
            return await _context.Branches
                .FirstOrDefaultAsync(b => b.ExternalId == externalId);
        }
    }
}
