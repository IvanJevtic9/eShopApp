using eShopApp.Catalog.App.IoC;
using Microsoft.EntityFrameworkCore;
using eShopApp.Catalog.Application.Settings;
using eShopApp.Catalog.Infrastructure.DataAccess;
using eShopApp.Catalog.Domain.Repository;
using eShopApp.Catalog.Infrastructure.Repository;

namespace eShopApp.Catalog.Infrastructure.IoC
{
    public static class DatabaseExtensions
    {
        public static IServiceCollection AddUnitOfWork(this IServiceCollection services)
        {
            return services.AddScoped<IUnitOfWork, UnitOfWork>();
        }

        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            var appSettings = configuration.GetSettings<AppSettings>();

            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(appSettings.ConnectionString));

            return services;
        }
    }
}
