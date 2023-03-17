using eShopApp.Catalog.Domain.Repository;
using eShopApp.Catalog.Infrastructure.DataAccess;
using eShopApp.Catalog.Domain.Entitites;

namespace eShopApp.Catalog.Infrastructure.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext db) : base(db)
        { }
    }
}
