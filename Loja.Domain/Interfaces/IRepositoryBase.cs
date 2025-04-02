using Loja.Domain.Entities;
using System.Linq.Expressions;

namespace Loja.Domain.Interfaces
{
    public interface IRepositoryBase<TEntity> where TEntity : EntityBase
    {
        Task<TEntity> GetByIdAsync(Guid id);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);
        Task AddAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task RemoveAsync(Guid id);
        Task<int> SaveChangesAsync();
    }
}
