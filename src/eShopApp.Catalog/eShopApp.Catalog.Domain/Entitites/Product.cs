using eShopApp.Catalog.Domain.Abstractions;

namespace eShopApp.Catalog.Domain.Entitites
{
    public sealed class Product : AggregateRoot
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public decimal UnitPrice { get; private set; }
        public string PictureFileName { get; private set; }
        public string PictureUri { get; private set; }
        public Guid BrandId { get; private set; }
        public Brand Brand { get; private set; }
        public Guid CategoryId { get; private set; }
        public Category Category { get; private set; }
        public int AvailableStock { get; private set; }

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

        public static Product Create(
            string name,
            string description,
            decimal unitPrice,
            string pictureFileName,
            string pictureUri,
            Brand brand,
            Category category,
            int availableStock)
        {
            // Validation

            return new Product(Guid.NewGuid(), name, description, unitPrice, pictureFileName, pictureUri, brand, category, availableStock);
        }

        public static Brand CreateNewBrand(string brandName)
        {
            return Brand.Create(brandName);
        }

        public static void UpdateBrand(Brand brand, string brandName)
        {
            brand.Update(brandName);
        }

        public static Category CreateNewCategory(string categoryName)
        {
            return Category.Create(categoryName);
        }

        public static void UpdateCategory(Category category, string categoryName)
        {
            category.Update(categoryName);
        }
    }
}
