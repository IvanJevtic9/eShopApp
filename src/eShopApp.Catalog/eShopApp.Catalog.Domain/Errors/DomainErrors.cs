using eShopApp.Catalog.Domain.Entitites;
using eShopApp.Catalog.Domain.Shared;

namespace eShopApp.Catalog.Domain.Errors
{
    public static class DomainErrors
    {
        public static readonly Error BrandNameValidationError = new Error(
                "Brand.NameRequiredField",
                $"{nameof(Brand.Name)} is required field.");

        public static readonly Error CategoryNameValidationError = new Error(
                "Category.NameRequiredField",
                $"{nameof(Category.Name)} is required field.");

        public static readonly Error ProductNameValidationError = new Error(
                "Product.NameRequiredField",
                $"{nameof(Product.Name)} is required field.");

        public static readonly Error ProductBrandValidationError = new Error(
                "Product.BrandRequiredField",
                $"{nameof(Product.Brand)} is required field.");

        public static readonly Error ProductCategoryValidationError = new Error(
                "Product.CategoryRequiredField",
                $"{nameof(Product.Category)} is required field.");
    }
}
