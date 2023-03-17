using eShopApp.Catalog.Domain.Repository;
using eShopApp.Catalog.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore.Storage;

namespace eShopApp.Catalog.Infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;

        public IBrandRepository Brand { get; init; }
        public ICategoryRepository Category { get; init; }
        public IProductRepository Product { get; init; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _db = context;

            Brand = new BrandRepository(_db);
            Category = new CategoryRepository(_db);
            Product = new ProductRepository(_db);
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            return await _db.Database.BeginTransactionAsync(cancellationToken);
        }

        public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
        {
            await _db.Database.CommitTransactionAsync(cancellationToken);
        }

        public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
        {
            await _db.Database.RollbackTransactionAsync(cancellationToken);
        }

        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
