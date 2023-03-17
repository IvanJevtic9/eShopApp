using eShopApp.Catalog.App.Middelware;

namespace eShopApp.Catalog.App.IoC
{
    public static class MiddelwareExtensions
    {
        public static IApplicationBuilder UseExceptionMiddelware(this IApplicationBuilder services)
        {
            return services.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
