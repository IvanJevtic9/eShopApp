using Microsoft.OpenApi.Models;
using eShopApp.Catalog.Application;
using System.Reflection;

namespace eShopApp.Catalog.App.IoC
{
    public static class ConfigurationExtensions
    {
        public static T GetSettings<T>(this IConfiguration configuration) where T : new()
        {
            var settings = new T();

            configuration.GetSection(typeof(T).Name)
                .Bind(settings);

            return settings;
        }

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

            services.AddControllers();
            services.AddMvc();

            return services;
        }

        public static IServiceCollection AddMediatR(this IServiceCollection services)
        {
            services.AddMediatR(conf =>
            {
                conf.RegisterServicesFromAssembly(typeof(ReferenceClass).GetTypeInfo().Assembly);
            });

            return services;
        }
    }
}
