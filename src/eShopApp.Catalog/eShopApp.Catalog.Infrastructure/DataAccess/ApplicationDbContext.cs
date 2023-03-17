using Microsoft.EntityFrameworkCore;
using eShopApp.Catalog.Domain.Entitites;

namespace eShopApp.Catalog.Infrastructure.DataAccess
{
    public sealed class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(builder =>
            {
                builder.ToTable("Product", "Catalog");

                builder.HasKey(u => u.Id);

                builder.Property(u => u.Name)
                 .IsRequired();

                builder.Property(u => u.UnitPrice)
                 .IsRequired();

                builder.HasOne(u => u.Brand)
                 .WithMany()
                 .HasForeignKey(u => u.BrandId)
                 .IsRequired();

                builder.HasOne(u => u.Category)
                 .WithMany()
                 .HasForeignKey(u => u.CategoryId)
                 .IsRequired();
            });

            modelBuilder.Entity<Brand>(builder =>
            {
                builder.ToTable("Brand", "Catalog");

                builder.HasKey(u => u.Id);

                builder.Property(u => u.Name)
                    .IsRequired();
            });

            modelBuilder.Entity<Category>(builder =>
            {
                builder.ToTable("Category", "Catalog");

                builder.HasKey(u => u.Id);

                builder.Property(u => u.Name)
                    .IsRequired();
            });
        }
    }
}
