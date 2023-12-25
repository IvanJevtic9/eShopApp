using MediatR;
using eShopApp.Shared.Primitives;
using eShopApp.Catalog.Domain.Entities;
using eShopApp.Catalog.Infrastructure.DataAccess.Base;
using eShopApp.Catalog.Application.Implementation.Commands;

namespace eShopApp.Catalog.Application.Implementation.Handlers
{
    public sealed class CreateBrandHandler : IRequestHandler<CreateBrandCommand, Result<Unit>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateBrandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Unit>> Handle(CreateBrandCommand request, CancellationToken cancellationToken)
        {
            var brand = Product.CreateNewBrand(request.Name);

            if (brand.IsFailure)
            {
                return new Result<Unit>(brand.GetError());
            }

            await _unitOfWork
                    .GetGenericRepository<Brand>()
                    .AddAsync(brand.GetValue());
            await _unitOfWork.SaveChangesAsync();

            return new Result<Unit>(Unit.Value);
        }
    }
}
