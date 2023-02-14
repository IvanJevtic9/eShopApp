using eShopApp.Catalog.Domain.Abstractions;

namespace eShopApp.Catalog.Domain.Entitites
{
    public sealed class Category : Entity
    {
        public string Name { get; private set; }

        private Category(Guid id) : base(id)
        { }

        internal static Category Create(string name)
        {
            var category = new Category(Guid.NewGuid());

            if (name is null)
            {
                // validation
            }

            category.Name = name;

            return category;
        }

        internal void Update(string name)
        {
            if(name is null)
            {
                // validation
            }

            Name = name;
        }
    }
}
