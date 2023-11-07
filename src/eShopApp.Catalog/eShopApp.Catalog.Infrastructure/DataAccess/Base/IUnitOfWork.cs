using eShopApp.Shared.DDAbstraction;
using eShopApp.Shared.Repository.Base;

namespace eShopApp.Catalog.Infrastructure.DataAccess.Base
{
    public interface IUnitOfWork
    {
        IRepository<TEntity> GetGenericRepository<TEntity>() where TEntity : Entity;
        Task SaveChangesAsync();
    }
}
