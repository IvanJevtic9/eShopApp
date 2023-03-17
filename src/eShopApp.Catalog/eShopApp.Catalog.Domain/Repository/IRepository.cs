using eShopApp.Catalog.Domain.Abstractions;
using System.Linq.Expressions;

namespace eShopApp.Catalog.Domain.Repository
{
    public interface IRepository<TEntity> where TEntity : Entity
    {
        Task<TEntity> GetAsync(Guid id);
        Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> filter);
        Task CreateAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task RemoveAsync(Guid id);
        Task<IReadOnlyCollection<TEntity>> GetAllAsync();
        Task<IReadOnlyCollection<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> filter);
    }
}
