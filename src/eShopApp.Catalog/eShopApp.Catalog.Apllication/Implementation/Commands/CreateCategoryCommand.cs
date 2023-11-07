using MediatR;
using eShopApp.Shared.Primitives;

namespace eShopApp.Catalog.Apllication.Implementation.Commands
{
    public sealed record CreateCategoryCommand(string Name) : IRequest<Result<Unit>>;
}
