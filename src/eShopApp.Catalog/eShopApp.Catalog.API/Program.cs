using Serilog;
using eShopApp.Shared.Extensions;
using eShopApp.Catalog.Apllication.IoT;
using eShopApp.Catalog.Infrastructure.IoT;
using eShopApp.Catalog.Infrastructure.DataAccess;

using ConfigurationExtensions = eShopApp.Shared.Extensions.ConfigurationExtensions;

// Configure host
IHost host = Host.CreateDefaultBuilder(args)
    .UseEnvironment(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development")
    .ConfigureAppConfiguration((hostingService, configuration) =>
    {
        var environmentName = hostingService.HostingEnvironment.EnvironmentName;

        configuration
            .AddJsonFile("appsettings.json")
            .AddJsonFile($"appsettings.{environmentName}.json", optional: true, true)
            .AddEnvironmentVariables();
    })
    .UseSerilog()
    .ConfigureWebHostDefaults(webBuilder =>
    {
        webBuilder.ConfigureServices((hostingContext, services) =>
        {
            ConfigurationExtensions.ConfigureLogging(hostingContext.Configuration);

            services.AddEndpointsApiExplorer();
            services.RegisterSettings(hostingContext.Configuration);
            services.AddSwaggerConfig("EShop Catalog API", "v1");
            services.AddDatabase<CatalogDbContext>(hostingContext.Configuration);
            services.RegisterHostedServices();
            services.RegisterMediator();
            services.RegisterRepository();
        });

        webBuilder.Configure((hostingContext, app) =>
        {
            app.ApplicationServices.RunMigration<CatalogDbContext>();

            //if (hostingContext.HostingEnvironment.IsDevelopment())
            //{
            app.UseSwagger();
            app.UseSwaggerUI();
            //}

            app.UseHttpsRedirection();
            app.UseSerilogRequestLogging();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        });
    })
    .Build();

try
{
    Log.Information("Application starting up.");
    await host.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "The application failed to start correctly");
}
finally
{
    Log.CloseAndFlush();
}