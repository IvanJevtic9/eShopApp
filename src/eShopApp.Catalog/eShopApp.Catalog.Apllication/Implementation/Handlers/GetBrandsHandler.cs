using MediatR;
using eShopApp.Shared.Primitives;
using eShopApp.Catalog.Domain.Entities;
using eShopApp.Catalog.Apllication.Implementation.Queries;
using eShopApp.Catalog.Infrastructure.DataAccess.Base;

namespace eShopApp.Catalog.Apllication.Implementation.Handlers
{
    public sealed class GetBrandsHandler : IRequestHandler<GetBrandsQuery, Result<List<Brand>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetBrandsHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<List<Brand>>> Handle(GetBrandsQuery request, CancellationToken cancellationToken)
        {
            var brands = await _unitOfWork
                .GetGenericRepository<Brand>()
                .GetAllAsync();

            return new Result<List<Brand>>(brands.ToList());
        }
    }
}
