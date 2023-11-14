using Autofac;
using Serilog;
using eShopApp.Shared.Extensions;
using Autofac.Extensions.DependencyInjection;
using eShopApp.Basket.Infrastructure.DataAccess;

IConfiguration _configuration = null;

// Configure host
IHost host = Host.CreateDefaultBuilder(args)
    .UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .UseEnvironment(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development")
    .ConfigureAppConfiguration((hostingService, configuration) =>
    {
        var environmentName = hostingService.HostingEnvironment.EnvironmentName;

        configuration
            .AddJsonFile("appsettings.json")
            .AddJsonFile($"appsettings.{environmentName}.json", optional: true, true)
            .AddEnvironmentVariables();
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
        builder.AddSwaggerConfig("EShop Basket API", "v1");
        builder.RegisterSettings(_configuration);
        builder.AddDatabase<BasketDbContext>(_configuration);
    })
    .ConfigureWebHostDefaults(webBuilder =>
    {
        webBuilder.ConfigureServices((hostingContext, services) =>
        {
            services.AddEndpointsApiExplorer();
        });
        
        webBuilder.Configure((hostingContext, app) =>
        {
            var lifetimeScope = app.ApplicationServices.GetRequiredService<ILifetimeScope>();
            lifetimeScope.RunMigration<BasketDbContext>();

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