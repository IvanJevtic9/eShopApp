using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using eShopApp.Catalog.Domain.Repository;
using eShopApp.Catalog.Domain.Abstractions;
using eShopApp.Catalog.Infrastructure.DataAccess;

namespace eShopApp.Catalog.Infrastructure.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity
    {
        private readonly DbSet<TEntity> _dbSet;

        public Repository(ApplicationDbContext db)
        {
            _dbSet = db.Set<TEntity>();
        }

        public async Task CreateAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task<IReadOnlyCollection<TEntity>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<IReadOnlyCollection<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> filter)
        {
            return await _dbSet
                .Where(filter)
                .ToListAsync();
        }

        public async Task<TEntity> GetAsync(Guid id)
        {
            return await _dbSet.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> filter)
        {
            return await _dbSet.FirstOrDefaultAsync(filter);
        }

        public async Task RemoveAsync(Guid id)
        {
            var entity = await _dbSet.FirstOrDefaultAsync(x => x.Id == id);

            await UpdateAsync(entity);
        }

        public async Task UpdateAsync(TEntity entity)
        {
            _dbSet.Remove(entity);
        }
    }
}
