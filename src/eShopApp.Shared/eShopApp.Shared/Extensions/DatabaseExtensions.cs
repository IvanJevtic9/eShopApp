using Autofac;
using eShopApp.Shared.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace eShopApp.Shared.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class DatabaseExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder"></param>
        /// <param name="configuration"></param>
        /// <param name="interceptors"></param>
        /// <returns></returns>
        public static ContainerBuilder AddDatabase<T>(
            this ContainerBuilder builder,
            IConfiguration configuration,
            params IInterceptor[] interceptors) where T : DbContext
        {
            var connectionString = configuration
                .BindSection<ConnectionStrings>()
                .DatabaseConnection;

            // Register the DbContext
            builder.Register(container =>
            {
                var optionsBuilder = new DbContextOptionsBuilder<T>();
                optionsBuilder.UseSqlServer(connectionString)
                              .AddInterceptors(interceptors);

                return (T)Activator.CreateInstance(typeof(T), optionsBuilder.Options);
            })
            .AsSelf()
            .InstancePerLifetimeScope();

            return builder;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="autofac"></param>
        public static void RunMigration<T>(this ILifetimeScope autofac) 
            where T : DbContext
        {
            using var scope = autofac.BeginLifetimeScope();
            var context = scope.Resolve<T>();

            var pendingMigrations = context.Database.GetPendingMigrations();
            if (pendingMigrations.Any())
            {
                context.Database.Migrate();
            }
        }
    }
}