using MediatR;
using eShopApp.Shared.Primitives;

namespace eShopApp.Catalog.Apllication.Implementation.Commands
{
    public sealed record CreateBrandCommand(string Name) : IRequest<Result<Unit>>;
}
