using Microsoft.OpenApi.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Routing;
using Autofac;
using eShopApp.Shared.Modules;

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
        /// <param name="builder"></param>
        /// <param name="apiName"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public static ContainerBuilder AddSwaggerConfig(
            this ContainerBuilder builder,
            string apiName,
            string apiVersion)
        {
            builder.RegisterModule(new SwaggerModule(apiName, apiVersion));

            return builder;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection ConfigureSharedServices(this IServiceCollection services)
        {
            services.AddControllers();
            services.AddMvc();
            services.Configure<RouteOptions>(options =>
            {
                options.LowercaseUrls = true;
            });

            return services;
        }
    }
}
