using Autofac;
using eShopApp.Shared.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace eShopApp.Shared.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class SettingExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static ContainerBuilder RegisterSettings(this ContainerBuilder builder, IConfiguration configuration)
        {
            var connectionStrings = configuration.BindSection<ConnectionStrings>();

            builder.RegisterInstance(connectionStrings).SingleInstance();

            return builder;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static T BindSection<T>(this IConfiguration configuration) where T : new()
        {
            var section = new T();

            configuration.GetSection(typeof(T).Name)
                .Bind(section);

            return section;
        }
    }
}
