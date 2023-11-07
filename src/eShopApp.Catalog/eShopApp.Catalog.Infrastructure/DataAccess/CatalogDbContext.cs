using Microsoft.EntityFrameworkCore;
using eShopApp.Catalog.Domain.Entities;

namespace eShopApp.Catalog.Infrastructure.DataAccess
{
    public class CatalogDbContext : DbContext
    {
        public CatalogDbContext(DbContextOptions dbOptions) : base(dbOptions)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(builder =>
            {
                builder.ToTable("Products", "Catalog");

                builder.HasKey(u => u.Id);

                builder.Property(u => u.Name)
                 .IsRequired();

                builder.HasIndex(x => x.Name)
                    .IsUnique();

                builder.Property(u => u.Price)
                 .HasColumnType("decimal(18,2)")
                 .IsRequired();

                builder.HasOne(u => u.Brand)
                 .WithMany()
                 .HasForeignKey(u => u.BrandId)
                 .OnDelete(DeleteBehavior.Cascade);

                builder.HasOne(u => u.Category)
                 .WithMany()
                 .HasForeignKey(u => u.CategoryId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Brand>(builder =>
            {
                builder.ToTable("Brands", "Catalog");

                builder.HasKey(u => u.Id);

                builder.HasIndex(x => x.Name)
                    .IsUnique();

                builder.Property(u => u.Name)
                    .IsRequired();
            });

            modelBuilder.Entity<Category>(builder =>
            {
                builder.ToTable("Categories", "Catalog");

                builder.HasKey(u => u.Id);

                builder.HasIndex(x => x.Name)
                    .IsUnique();

                builder.Property(u => u.Name)
                    .IsRequired();
            });

            modelBuilder.Entity<OutboxMessage>(builder =>
            {
                builder.ToTable("OutboxMessages", "Infastructure");

                builder.HasKey(u => u.Id);

                builder.Property(u => u.Type)
                    .IsRequired();

                builder.Property(u => u.OccuredOnUtc)
                    .IsRequired();
            });
        }
    }
}
