using eShopApp.Shared.Primitives;
using eShopApp.Shared.DDAbstraction;

namespace eShopApp.Catalog.Domain.Entities
{
    public sealed class Category : Entity
    {
        public string Name { get; private set; }

        private Category() : base(Guid.NewGuid())
        { }

        private Category(string name) : this()
        {
            Name = name;
        }

        internal static Result<Category> Create(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return new Result<Category>(DomainErrors.CategoryRequiredNameValidationError);
            }

            return new Result<Category>(new Category(name));
        }

        public bool Update(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                Name = name;

                return true;
            }

            return false;
        }
    }
}
