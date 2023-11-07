using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using eShopApp.Shared.DDAbstraction;
using eShopApp.Shared.Repository.Base;

namespace eShopApp.Shared.Repository
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity
    {
        protected readonly DbSet<TEntity> _dbSet;

        /// <summary>
        /// Initializes a new instance of the repository with the specified database context.
        /// </summary>
        /// <param name="db">The database context to be used for data operations.</param>
        public Repository(DbContext db)
        {
            _dbSet = db.Set<TEntity>();
        }

        /// <summary>
        /// Adds a new entity to the database asynchronously.
        /// </summary>
        /// <param name="entity">The entity to be added.</param>
        public async Task AddAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
        }

        /// <summary>
        /// Adds a collection of entities to the database asynchronously.
        /// </summary>
        /// <param name="entities">The collection of entities to be added.</param>
        public async Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }

        /// <summary>
        /// Asynchronously checks if any entity in the database satisfies the specified predicate.
        /// </summary>
        /// <param name="predicate">The predicate used to filter entities.</param>
        /// <returns>
        /// The task result is true if any entity satisfies the predicate; otherwise, false.
        /// </returns>
        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }

        /// <summary>
        /// Asynchronously retrieves a collection of entities from the database based on the specified predicate, ordering, and included properties.
        /// </summary>
        /// <param name="predicate">The predicate used to filter entities. (Optional)</param>
        /// <param name="orderBy">The ordering function used to sort entities. (Optional)</param>
        /// <param name="includeProperties">A comma-separated list of navigation properties to include in the result. (Optional)</param>
        /// <returns>
        /// The task result is a collection of entities that match the specified criteria.
        /// </returns>
        public async Task<IEnumerable<TEntity>> GetAllAsync(
            Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = null)
        {
            IQueryable<TEntity> query = _dbSet;

            if (predicate != null)
            {
                query = await Task.Run(() => { return _dbSet.Where(predicate); });
            }

            if (includeProperties != null)
            {
                foreach (var property in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(property);
                }
            }

            if (orderBy != null)
            {
                return orderBy(query);
            }

            return query.ToList();
        }

        /// <summary>
        /// Asynchronously retrieves an entity from the database based on the specified ID.
        /// </summary>
        /// <param name="id">The ID of the entity to retrieve.</param>
        /// <returns>
        /// The task result is the entity with the specified ID, or null if not found.
        /// </returns>
        public async Task<TEntity> GetByIdAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        /// <summary>
        /// Asynchronously retrieves the first entity that matches the specified predicate from the database.
        /// </summary>
        /// <param name="predicate">The predicate used to filter entities. (Optional)</param>
        /// <param name="includeProperties">A comma-separated list of navigation properties to include in the result. (Optional)</param>
        /// <returns>
        /// The task result is the first entity that matches the specified predicate, or null if not found.
        /// </returns>
        public async Task<TEntity> GetFirstOrDefaultAsync(
            Expression<Func<TEntity, bool>> predicate = null,
            string includeProperties = null)
        {
            IQueryable<TEntity> query = _dbSet;

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (includeProperties != null)
            {
                foreach (var property in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(property);
                }
            }

            return await query.FirstOrDefaultAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task RemoveAsync(Guid id)
        {
            var entity = await _dbSet.FindAsync(id);

            _dbSet.Remove(entity);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        public void Remove(TEntity entity)
        {
            _dbSet.Remove(entity);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entities"></param>
        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            _dbSet.RemoveRange(entities);
        }
    }
}