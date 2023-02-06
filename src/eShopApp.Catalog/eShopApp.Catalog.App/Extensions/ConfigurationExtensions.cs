using Microsoft.OpenApi.Models;

namespace eShopApp.Catalog.App.Extensions
{
    public static class ConfigurationExtensions
    {
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "EShop Catalog API",
                    Version = "v1",
                    Contact = new OpenApiContact
                    {
                        Name = "Ivan Jevtic",
                        Email = "ijevtic459@gmail.com"
                    }
                });
            });

            return services;
        }
    }
}
