using MediatR;
using Microsoft.AspNetCore.Mvc;
using eShopApp.Shared.Extensions;
using eShopApp.Shared.Controller;
using eShopApp.Catalog.Domain.Entities;
using eShopApp.Catalog.Application.Models.Requests;
using eShopApp.Catalog.Apllication.Implementation.Queries;
using eShopApp.Catalog.Apllication.Implementation.Commands;

namespace eShopApp.Catalog.API.Controllers
{
    public class CatalogController : ApiController
    {
        public CatalogController(IMediator mediator) : base(mediator)
        { }

        [HttpGet("brand")]
        public async Task<ActionResult<List<Brand>>> GetAllBrands() =>
            (await _mediator.Send(new GetBrandsQuery()))
                .ToResponse();

        [HttpPost("brand")]
        public async Task<ActionResult<Unit>> CreateBrand([FromBody] CreateBrand request) =>
            (await _mediator.Send(new CreateBrandCommand(request.Name)))
                    .ToResponse();

        [HttpGet("category")]
        public async Task<ActionResult<List<Category>>> GetAllCategories() =>
            (await _mediator.Send(new GetCategoriesQuery()))
                    .ToResponse();

        [HttpPost("category")]
        public async Task<ActionResult<Unit>> CreateCategory([FromBody] CreateCategory request) =>
            (await _mediator.Send(new CreateCategoryCommand(request.Name)))
                    .ToResponse();
    }
}
