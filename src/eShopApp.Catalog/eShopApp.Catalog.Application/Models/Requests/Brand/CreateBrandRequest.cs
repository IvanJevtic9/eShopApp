using Microsoft.AspNetCore.Http;

namespace eShopApp.Catalog.Application.Models.Requests.Brand
{
    public sealed record CreateBrandRequest(string Name);
}
