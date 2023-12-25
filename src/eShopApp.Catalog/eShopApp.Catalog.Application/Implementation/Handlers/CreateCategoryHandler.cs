using MediatR;
using eShopApp.Shared.Primitives;
using eShopApp.Catalog.Domain.Entities;
using eShopApp.Catalog.Infrastructure.DataAccess.Base;
using eShopApp.Catalog.Application.Implementation.Commands;

namespace eShopApp.Catalog.Application.Implementation.Handlers
{
    public sealed class CreateCategoryHandler : IRequestHandler<CreateCategoryCommand, Result<Unit>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateCategoryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Unit>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = Product.CreateNewCategory(request.Name);

            if (category.IsFailure)
            {
                return new Result<Unit>(category.GetError());
            }

            await _unitOfWork
                    .GetGenericRepository<Category>()
                    .AddAsync(category.GetValue());
            await _unitOfWork.SaveChangesAsync();

            return new Result<Unit>(Unit.Value);
        }
    }
}
