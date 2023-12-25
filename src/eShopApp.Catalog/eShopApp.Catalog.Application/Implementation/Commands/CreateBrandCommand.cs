using MediatR;
using eShopApp.Shared.Primitives;

namespace eShopApp.Catalog.Application.Implementation.Commands
{
    public sealed record CreateBrandCommand(string Name) : IRequest<Result<Unit>>;
}
