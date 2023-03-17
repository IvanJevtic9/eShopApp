using eShopApp.Catalog.Domain.Abstractions;
using eShopApp.Catalog.Domain.Errors;
using eShopApp.Catalog.Domain.Shared;

namespace eShopApp.Catalog.Domain.Entitites
{
    public sealed class Brand : Entity
    {
        public string Name { get; private set; }
        public string PictureUri { get; private set; }

        private Brand() : base(Guid.NewGuid())
        { }

        internal static Result<Brand> Create(string name, string pictureUri)
        {
            if (name is null)
            {
                return new Result<Brand>(DomainErrors.BrandNameValidationError);
            }

            var brand = new Brand()
            {
                Name = name,
                PictureUri = pictureUri
            };

            return new Result<Brand>(brand);
        }

        internal void Update(string name, string pictureUri)
        {
            if (name is not null)
            {
                Name = name;
            }

            if (pictureUri is not null)
            {
                PictureUri = pictureUri;
            }
        }
    }
}
