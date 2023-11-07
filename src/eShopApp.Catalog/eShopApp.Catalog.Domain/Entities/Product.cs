using eShopApp.Shared.Primitives;
using eShopApp.Shared.DDAbstraction;
using eShopApp.Catalog.Domain.DomainEvents;

namespace eShopApp.Catalog.Domain.Entities
{
    public class Product : AggregateRoot
    {
        public string Name { get; private set; } = null!;
        public decimal Price { get; private set; }
        public string Description { get; private set; } = null!;
        public Guid BrandId { get; private set; }
        public Brand Brand { get; private set; } = null!;
        public Guid CategoryId { get; private set; }
        public Category Category { get; private set; } = null!;

        private Product() : base(Guid.NewGuid())
        { }

        private Product(
            string name,
            decimal price,
            string description,
            Brand brand,
            Category category) : this()
        {
            Name = name;
            Price = price;
            Description = description;
            Brand = brand;
            Category = category;

            RaiseDomainEvent(new ProductCreatedDomainEvent(Id));
        }

        private Result<Brand> CreateBrand(string name)
        {
            // here we can raise domain event

            return Brand.Create(name);
        }

        private Result<Category> CreateCategory(string name)
        {
            // here we can raise domain event

            return Category.Create(name);
        }

        public static Result<Product> Create(
            string name,
            decimal price,
            string description,
            Brand brand,
            Category category)
        {
            if (string.IsNullOrEmpty(name))
            {
                return new Result<Product>(
                    DomainErrors.ProductRequiredNameValidationError
                );
            }

            if (price <= 0)
            {
                return new Result<Product>(
                    DomainErrors.ProductPriceMustBeGreaterThenZeroValidationError
                );
            }

            if (brand is null)
            {
                return new Result<Product>(
                    DomainErrors.BrandRequiredNameValidationError
                );
            }

            if (category is null)
            {
                return new Result<Product>(
                    DomainErrors.CategoryRequiredNameValidationError
                );
            }

            return new Result<Product>(
                new Product(
                    name,
                    price,
                    description,
                    brand,
                    category));
        }

        public void Update(
            string name,
            decimal? price,
            string description,
            Brand brand,
            Category category)
        {
            var isUpdated = false;
            
            var oldPrice = Price;
            var oldName = Name;

            if (!string.IsNullOrEmpty(name))
            {
                Name = name;
                isUpdated = true;
            }

            if (price.HasValue)
            {
                Price = price.Value;
                isUpdated = true;
            }

            if (!string.IsNullOrEmpty(description))
            {
                Description = description;
            }

            if (brand is not null && brand.Id != Guid.Empty)
            {
                BrandId = brand.Id;
                Brand = brand;
                isUpdated = true;
            }

            if (category is not null && category.Id != Guid.Empty)
            {
                CategoryId = category.Id;
                Category = category;
                isUpdated = true;
            }

            if (isUpdated)
            {
                RaiseDomainEvent(new ProductUpdatedDomainEvent(Id, oldName, oldPrice));
            }
        }


        public static Result<Brand> CreateNewBrand(string name)
        {
            Product pr = new();

            return pr.CreateBrand(name);
        }

        public static Result<Category> CreateNewCategory(string name)
        {
            Product pr = new();

            return pr.CreateCategory(name);
        }
    }
}
