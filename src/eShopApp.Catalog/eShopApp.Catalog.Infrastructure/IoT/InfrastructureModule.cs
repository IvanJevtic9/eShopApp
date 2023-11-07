using Microsoft.Extensions.DependencyInjection;
using eShopApp.Catalog.Infrastructure.DataAccess;
using eShopApp.Catalog.Infrastructure.DataAccess.Base;

namespace eShopApp.Catalog.Infrastructure.IoT
{
    public static class InfrastructureModule
    {
        public static IServiceCollection RegisterRepository(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, CatalogUnitOfWork>();

            return services;
        }
    }
}
