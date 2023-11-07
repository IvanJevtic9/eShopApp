using eShopApp.Shared.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <param name="interceptors"></param>
        /// <returns></returns>
        public static IServiceCollection AddDatabase<T>(
            this IServiceCollection services,
            IConfiguration configuration,
            params IInterceptor[] interceptors)
            where T : DbContext
        {
            var connectionString = configuration.BindSection<ConnectionStrings>().DatabaseConnection;

            services.AddDbContext<T>(options =>
            {
                options.UseSqlServer(connectionString)
                    .AddInterceptors(interceptors);
            });

            return services;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceProvider"></param>
        public static void RunMigration<T>(this IServiceProvider serviceProvider) 
            where T : DbContext
        {
            using var scope = serviceProvider.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<T>();
            var pendingMigrations = context.Database.GetPendingMigrations();
            if (pendingMigrations.Any())
            {
                context.Database.Migrate();
            }
        }
    }
}