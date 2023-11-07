using MediatR;
using eShopApp.Shared.Primitives;
using eShopApp.Catalog.Domain.Entities;

namespace eShopApp.Catalog.Apllication.Implementation.Queries
{
    public sealed record GetCategoriesQuery() : IRequest<Result<List<Category>>>;
}
