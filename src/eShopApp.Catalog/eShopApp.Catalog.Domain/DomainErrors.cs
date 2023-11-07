using eShopApp.Shared.Primitives;
using eShopApp.Catalog.Domain.Entities;

namespace eShopApp.Catalog.Domain
{
    public static class DomainErrors
    {
        public static readonly Error BrandRequiredNameValidationError = new Error(
            "brand_name",
            $"{nameof(Brand.Name)} is required field.");

        public static readonly Error CategoryRequiredNameValidationError = new Error(
            "category_name",
            $"{nameof(Category.Name)} is required field.");

        public static readonly Error ProductRequiredNameValidationError = new Error(
            "product_name",
            $"{nameof(Product.Name)} is required field.");

        public static readonly Error ProductRequiredBrandValidationError = new Error(
            "product_brand",
            $"{nameof(Product.Brand)} is required field.");

        public static readonly Error ProductRequiredCategoryValidationError = new Error(
            "product_category",
            $"{nameof(Product.Category)} is required field.");

        public static readonly Error ProductPriceMustBeGreaterThenZeroValidationError = new Error(
            "product_unitPrice",
            $"{nameof(Product.Price)} must be greater then zero.");
    }
}
