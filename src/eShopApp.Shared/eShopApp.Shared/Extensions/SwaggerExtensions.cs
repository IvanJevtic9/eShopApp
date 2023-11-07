using Microsoft.OpenApi.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Routing;

namespace eShopApp.Shared.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class SwaggerExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="apiName"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public static IServiceCollection AddSwaggerConfig(
            this IServiceCollection services,
            string apiName,
            string apiVersion)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(apiVersion, new OpenApiInfo
                {
                    Title = apiName,
                    Version = apiVersion,
                    Contact = new OpenApiContact
                    {
                        Name = "Ivan Jevtić",
                        Email = "ijevtic459@gmail.com"
                    }
                });
            });

            services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
            services.AddControllers();
            services.AddMvc();

            return services;
        }
    }
}
