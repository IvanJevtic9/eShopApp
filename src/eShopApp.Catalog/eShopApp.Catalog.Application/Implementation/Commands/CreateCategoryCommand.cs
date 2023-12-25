using MediatR;
using eShopApp.Shared.Primitives;

namespace eShopApp.Catalog.Application.Implementation.Commands
{
    public sealed record CreateCategoryCommand(string Name) : IRequest<Result<Unit>>;
}
