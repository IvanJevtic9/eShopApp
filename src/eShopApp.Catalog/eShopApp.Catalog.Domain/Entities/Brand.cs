using eShopApp.Shared.Primitives;
using eShopApp.Shared.DDAbstraction;

namespace eShopApp.Catalog.Domain.Entities
{
    public class Brand : Entity
    {
        public string Name { get; private set; }

        private Brand() : base(Guid.NewGuid())
        { }

        private Brand(string name) : this()
        {
            Name = name;
        }

        internal static Result<Brand> Create(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return new Result<Brand>(DomainErrors.BrandRequiredNameValidationError);
            }

            return new Result<Brand>(new Brand(name));
        }

        public bool Update(string name)
        {
            if(!string.IsNullOrEmpty(name))
            {
                Name = name;

                return true;
            }

            return false;
        }
    }
}
