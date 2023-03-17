using Microsoft.EntityFrameworkCore.Storage;

namespace eShopApp.Catalog.Domain.Repository
{
    public interface IUnitOfWork
    {
        IBrandRepository Brand { get; init; }
        ICategoryRepository Category { get; init; }
        IProductRepository Product { get; init; }

        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
        Task CommitTransactionAsync(CancellationToken cancellationToken = default);
        Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
        Task SaveChangesAsync();
    }
}
