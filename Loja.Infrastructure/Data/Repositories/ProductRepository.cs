using Microsoft.EntityFrameworkCore;
using Loja.Domain.Entities;
using Loja.Domain.Interfaces;
using Loja.Infrastructure.Repositories;

namespace Loja.Infrastructure.Data.Repositories
{
    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Product> GetByExternalIdAsync(string externalId)
        {
            return await _context.Products
                .FirstOrDefaultAsync(p => p.ExternalId == externalId);
        }
    }
}
