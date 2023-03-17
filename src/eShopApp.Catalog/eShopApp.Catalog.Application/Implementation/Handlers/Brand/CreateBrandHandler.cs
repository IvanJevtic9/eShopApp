using MediatR;
using eShopApp.Catalog.Domain.Shared;
using eShopApp.Catalog.Domain.Entitites;
using eShopApp.Catalog.Domain.Repository;
using eShopApp.Catalog.Application.Models.Responses.Brand;
using eShopApp.Catalog.Application.Implementation.Commands.Brand;

namespace eShopApp.Catalog.Application.Implementation.Handlers.Brand
{
    public sealed class CreateBrandHandler : IRequestHandler<CreateBrandCommand, Result<CreateBrandResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateBrandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<CreateBrandResponse>> Handle(CreateBrandCommand request, CancellationToken cancellationToken)
        {
            var brand = Product.CreateNewBrand(request.Name, null);

            if (brand.IsFailure)
            {
                return new Result<CreateBrandResponse>(brand.GetError());
            }

            var value = brand.GetValue();

            await _unitOfWork.Brand.CreateAsync(value);
            await _unitOfWork.SaveChangesAsync();

            return new Result<CreateBrandResponse>(new CreateBrandResponse(
                value.Id,
                value.Name,
                value.PictureUri
            ));
        }
    }
}
