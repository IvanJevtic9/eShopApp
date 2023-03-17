using eShopApp.Catalog.Domain.Errors;
using eShopApp.Catalog.Domain.Shared;
using eShopApp.Catalog.Domain.Abstractions;

namespace eShopApp.Catalog.Domain.Entitites
{
    public sealed class Product : AggregateRoot
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public decimal UnitPrice { get; private set; }
        public string PictureUri { get; private set; }
        public string PictureFileName { get; private set; }
        public Guid BrandId { get; private set; }
        public Brand Brand { get; private set; }
        public Guid CategoryId { get; private set; }
        public Category Category { get; private set; }
        public int AvailableStock { get; private set; }

        private Product() : base(Guid.NewGuid())
        { }

        private Product(
            Guid id,
            string name,
            string description,
            decimal unitPrice,
            string pictureFileName,
            string pictureUri,
            Brand brand,
            Category category,
            int availableStock) : base(id)
        {
            Name = name;
            Description = description;
            UnitPrice = unitPrice;
            PictureFileName = pictureFileName;
            PictureUri = pictureUri;
            Brand = brand;
            Category = category;
            AvailableStock = availableStock;
        }

        public static Result<Product> Create(
            string name,
            string description,
            decimal unitPrice,
            string pictureFileName,
            string pictureUri,
            Brand brand,
            Category category,
            int availableStock)
        {
            if (name is null)
            {
                return new Result<Product>(DomainErrors.ProductNameValidationError);
            }

            if (brand is null)
            {
                return new Result<Product>(DomainErrors.ProductBrandValidationError);
            }

            if (category is null)
            {
                return new Result<Product>(DomainErrors.ProductCategoryValidationError);
            }

            var porduct = new Product(Guid.NewGuid(), name, description, unitPrice, pictureFileName, pictureUri, brand, category, availableStock);

            return new Result<Product>(porduct);
        }

        public static Result<Brand> CreateNewBrand(string brandName, string pictureUri)
        {
            return Brand.Create(brandName, pictureUri);
        }

        public static void UpdateBrand(Brand brand, string brandName, string pictureUri)
        {
            brand.Update(brandName, pictureUri);
        }

        public static Result<Category> CreateNewCategory(string categoryName, string iconUri)
        {
            return Category.Create(categoryName, iconUri);
        }

        public static void UpdateCategory(Category category, string categoryName, string IconUri)
        {
            category.Update(categoryName, IconUri);
        }
    }
}
