using Autofac;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.Extensions.DependencyInjection;

namespace eShopApp.Shared.Modules
{
    /// <summary>
    /// 
    /// </summary>
    public class SwaggerModule : Module
    {
        private readonly string _apiName;
        private readonly string _apiVersion;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="apiName"></param>
        /// <param name="apiVersion"></param>
        public SwaggerModule(string apiName, string apiVersion)
        {
            _apiName = apiName;
            _apiVersion = apiVersion;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(container =>
            {
                var services = new ServiceCollection();
                services.AddSwaggerGen(options =>
                {
                    options.SwaggerDoc(_apiVersion, new OpenApiInfo
                    {
                        Title = _apiName,
                        Version = _apiVersion,
                        Contact = new OpenApiContact
                        {
                            Name = "Ivan Jevtić",
                            Email = "ijevticL459@gmail.com"
                        }
                    });
                });

                return services
                    .BuildServiceProvider()
                    .GetServices<ISwaggerProvider>();
            })
            .AsSelf()
            .SingleInstance();
        }
    }
}
