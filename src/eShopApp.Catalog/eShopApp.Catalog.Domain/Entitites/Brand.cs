using eShopApp.Catalog.Domain.Abstractions;

namespace eShopApp.Catalog.Domain.Entitites
{
    public sealed class Brand : Entity
    {
        public string Name { get; private set; }

        private Brand(Guid id) : base(id)
        { }

        internal static Brand Create(string Name)
        {
            var brand = new Brand(Guid.NewGuid());

            if(Name is null)
            {
                // validation
            }

            brand.Name = Name;

            return brand;
        }

        internal void Update(string name)
        {
            if (name is null)
            {
                // validation
            }

            Name = name;
        }
    }
}
