using MediatR;
using eShopApp.Shared.Primitives;
using eShopApp.Catalog.Domain.Entities;
using eShopApp.Catalog.Infrastructure.DataAccess.Base;
using eShopApp.Catalog.Apllication.Implementation.Queries;

namespace eShopApp.Catalog.Application.Implementation.Handlers
{
    public sealed class GetCategoriesHandler : IRequestHandler<GetCategoriesQuery, Result<List<Category>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetCategoriesHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<List<Category>>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
        {
            var categories = await _unitOfWork
                .GetGenericRepository<Category>()
                .GetAllAsync();

            return new Result<List<Category>>(categories.ToList());
        }
    }
}
