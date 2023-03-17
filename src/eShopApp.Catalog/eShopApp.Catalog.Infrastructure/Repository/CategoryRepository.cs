using eShopApp.Catalog.Domain.Repository;
using eShopApp.Catalog.Infrastructure.DataAccess;
using eShopApp.Catalog.Domain.Entitites;

namespace eShopApp.Catalog.Infrastructure.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext db) : base(db)
        { }
    }
}
