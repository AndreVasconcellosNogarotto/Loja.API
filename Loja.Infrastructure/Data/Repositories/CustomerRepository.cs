using Microsoft.EntityFrameworkCore;
using Loja.Domain.Entities;
using Loja.Domain.Interfaces;
using Loja.Infrastructure.Repositories;

namespace Loja.Infrastructure.Data.Repositories
{
    public class CustomerRepository : RepositoryBase<Customer>, ICustomerRepository
    {
        public CustomerRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Customer> GetByExternalIdAsync(string externalId)
        {
            return await _context.Customers
                .FirstOrDefaultAsync(c => c.ExternalId == externalId);
        }
    }
}
