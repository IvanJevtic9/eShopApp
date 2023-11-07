using eShopApp.Shared.Repository;
using eShopApp.Shared.DDAbstraction;
using eShopApp.Shared.Repository.Base;
using eShopApp.Catalog.Infrastructure.DataAccess.Base;

namespace eShopApp.Catalog.Infrastructure.DataAccess
{
    internal class CatalogUnitOfWork : IUnitOfWork
    {
        private readonly CatalogDbContext _db;

        public CatalogUnitOfWork(CatalogDbContext catalogDbContext)
        {
            _db = catalogDbContext;
        }

        public IRepository<TEntity> GetGenericRepository<TEntity>() where TEntity : Entity
        {
            return new Repository<TEntity>(_db);
        }

        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
