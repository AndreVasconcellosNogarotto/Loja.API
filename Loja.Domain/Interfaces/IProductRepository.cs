using Loja.Domain.Entities;

namespace Loja.Domain.Interfaces
{
    public interface IProductRepository : IRepositoryBase<Product>
    {
        Task<Product> GetByExternalIdAsync(string externalId);
    }
}
