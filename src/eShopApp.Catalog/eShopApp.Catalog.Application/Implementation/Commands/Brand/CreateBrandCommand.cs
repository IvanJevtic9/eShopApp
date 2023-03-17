using eShopApp.Catalog.Application.Models.Responses.Brand;
using eShopApp.Catalog.Domain.Shared;
using MediatR;

namespace eShopApp.Catalog.Application.Implementation.Commands.Brand
{
    public sealed record CreateBrandCommand(string Name) : IRequest<Result<CreateBrandResponse>>;
}
