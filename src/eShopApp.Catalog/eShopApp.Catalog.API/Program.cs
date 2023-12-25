using Serilog;
using Autofac;
using eShopApp.Shared.Extensions;
using eShopApp.Catalog.Application.IoT;
using eShopApp.Catalog.Infrastructure.IoT;
using Autofac.Extensions.DependencyInjection;
using eShopApp.Catalog.Infrastructure.DataAccess;

IConfiguration _configuration = null;

// Configure host
IHost host = Host.CreateDefaultBuilder(args)
    .UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .UseEnvironment(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development")
    .ConfigureAppConfiguration((hostingService, configurationBuilder) =>
    {
        var environmentName = hostingService.HostingEnvironment.EnvironmentName;

        configurationBuilder
            .AddJsonFile("appsettings.json")
            .AddJsonFile($"appsettings.{environmentName}.json", optional: true, true)
            .AddEnvironmentVariables();

        _configuration = configurationBuilder.Build();
    })
    .UseSerilog((hostingContext, loggerConfiguration) =>
    {
        loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration)
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
            .MinimumLevel.Override("System", Serilog.Events.LogEventLevel.Warning);
    })
    .ConfigureContainer<ContainerBuilder>(builder =>
    {
        builder.AddSwaggerConfig("EShop Catalog API", "v1");
        builder.RegisterSettings(_configuration);
        builder.AddDatabase<CatalogDbContext>(_configuration);
    })
    .ConfigureWebHostDefaults(webBuilder =>
    {
        webBuilder.ConfigureServices((hostingContext, services) =>
        {
            services.AddEndpointsApiExplorer();
            services.RegisterHostedServices();
            services.RegisterMediator();
            services.RegisterRepository();
        });

        webBuilder.Configure((hostingContext, app) =>
        {
            var lifetimeScope = app.ApplicationServices.GetRequiredService<ILifetimeScope>();
            lifetimeScope.RunMigration<CatalogDbContext>();

            // Only for development
            app.UseSwagger();
            app.UseSwaggerUI();

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