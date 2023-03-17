using eShopApp.Catalog.Domain.Errors;
using eShopApp.Catalog.Domain.Shared;
using eShopApp.Catalog.Domain.Abstractions;

namespace eShopApp.Catalog.Domain.Entitites
{
    public sealed class Category : Entity
    {
        public string Name { get; private set; }
        public string IconUri { get; private set; }

        private Category() : base(Guid.NewGuid())
        { }

        internal static Result<Category> Create(string name, string iconUri)
        {
            if (name is null)
            {
                return new Result<Category>(DomainErrors.CategoryNameValidationError);
            }

            var category = new Category()
            {
                Name = name,
                IconUri = iconUri
            };

            return new Result<Category>(category);
        }

        internal void Update(string name, string iconUri)
        {
            if(name is not null)
            {
                Name = name;
            }

            if(iconUri is not null) 
            {
                IconUri = iconUri;
            }
        }
    }
}
