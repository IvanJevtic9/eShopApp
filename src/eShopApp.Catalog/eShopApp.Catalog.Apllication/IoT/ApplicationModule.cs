using eShopApp.Catalog.Apllication.HostedServices;
using Microsoft.Extensions.DependencyInjection;

namespace eShopApp.Catalog.Apllication.IoT
{
    public static class ApplicationModule
    {
        public static IServiceCollection RegisterMediator(this IServiceCollection services)
        {
            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssemblies(typeof(ApplicationModule).Assembly);
            });

            return services;
        }

        public static IServiceCollection RegisterHostedServices(this IServiceCollection services)
        {
            services.AddHostedService<OutboxProcessorService>();

            return services;
        }
    }
}
