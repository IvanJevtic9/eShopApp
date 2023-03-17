using eShopApp.Catalog.Domain.Repository;
using eShopApp.Catalog.Infrastructure.DataAccess;
using eShopApp.Catalog.Domain.Entitites;

namespace eShopApp.Catalog.Infrastructure.Repository
{
    public class BrandRepository : Repository<Brand>, IBrandRepository
    {
        public BrandRepository(ApplicationDbContext db) : base(db)
        { }
    }
}
