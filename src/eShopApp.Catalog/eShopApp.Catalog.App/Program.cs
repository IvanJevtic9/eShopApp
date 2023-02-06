
using eShopApp.Catalog.App.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwagger();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint($"/swagger/v1/swagger.json", "EShop Catalog API service");
});

app.UseHttpsRedirection();

app.MapGet("time", async () =>
{
    return Results.Ok(DateTime.UtcNow);
});

app.Run();
