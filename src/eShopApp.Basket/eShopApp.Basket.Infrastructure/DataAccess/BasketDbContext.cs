using Microsoft.EntityFrameworkCore;
using eShopApp.Basket.Domain.Entities;

namespace eShopApp.Basket.Infrastructure.DataAccess
{
    public class BasketDbContext : DbContext
    {
        public BasketDbContext(DbContextOptions dbOptions) : base(dbOptions)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cart>(builder =>
            {
                builder.ToTable("Carts", "Basket");

                builder.HasIndex(c => c.Id);

                builder.Property(c => c.UserId)
                    .IsRequired();

                builder.Property(c => c.CreatedDate) 
                    .IsRequired();

                builder.HasMany(c => c.Items)
                    .WithOne(ci => ci.Cart)
                    .HasForeignKey(ci => ci.CartId)
                    .IsRequired();
            });

            modelBuilder.Entity<CartItem>(builder =>
            {
                builder.ToTable("CartItems", "Basket");

                builder.HasIndex(c => c.Id);

                builder.Property(c => c.ProductId)
                    .IsRequired();

                builder.Property(c => c.ProductName)
                    .IsRequired();

                builder.Property(c => c.Price)
                    .IsRequired();

                builder.Property(c => c.Quantity)
                    .IsRequired();

                builder.HasOne(c => c.Cart)
                    .WithMany(ci => ci.Items)
                    .HasForeignKey(ci => ci.CartId)
                    .IsRequired();
            });
        }
    }
}
