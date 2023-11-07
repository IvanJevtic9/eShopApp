using Serilog;
using eShopApp.Shared.Extensions;
using eShopApp.Basket.Infrastructure.DataAccess;

using ConfigurationExtensions = eShopApp.Shared.Extensions.ConfigurationExtensions;

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
            services.AddSwaggerConfig("EShop Basket API", "v1");
            services.AddDatabase<BasketDbContext>(hostingContext.Configuration);
        });
        
        webBuilder.Configure((hostingContext, app) =>
        {
            app.ApplicationServices.RunMigration<BasketDbContext>();

            if (hostingContext.HostingEnvironment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseSerilogRequestLogging();
            app.UseRouting();
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