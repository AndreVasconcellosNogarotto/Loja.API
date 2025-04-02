using Loja.Domain.Entities;

namespace Loja.Domain.Interfaces
{
    public interface ICustomerRepository : IRepositoryBase<Customer>
    {
        Task<Customer> GetByExternalIdAsync(string externalId);
    }
}
