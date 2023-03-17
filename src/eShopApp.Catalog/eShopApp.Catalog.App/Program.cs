using eShopApp.Catalog.App.IoC;
using eShopApp.Catalog.Infrastructure.DataAccess;
using eShopApp.Catalog.Infrastructure.IoC;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwagger();
builder.Services.AddMediatR();
builder.Services.AddUnitOfWork();
builder.Services.AddDatabase(builder.Configuration);

var app = builder.Build();

// Migrate pending migrations
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    RunMigration(context);
}

// Configure the HTTP request pipeline.
app.UseExceptionMiddelware();
app.UseHttpsRedirection();
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint($"/swagger/v1/swagger.json", "EShop Catalog API service");
});
app.UseHttpsRedirection();
app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});
app.Run();

static void RunMigration(ApplicationDbContext context)
{
    var pendingMigrations = context.Database.GetPendingMigrations();

    if (pendingMigrations.Any())
    {
        context.Database.Migrate();
    }
}