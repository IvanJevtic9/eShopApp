using MediatR;
using Microsoft.AspNetCore.Mvc;
using eShopApp.Catalog.Application.Models.Requests.Brand;
using eShopApp.Catalog.Application.Implementation.Commands.Brand;
using eShopApp.Catalog.Application.Models.Responses.Brand;
using eShopApp.Catalog.App.Extensions;
using eShopApp.Catalog.Application.Models;

namespace eShopApp.Catalog.App.Controller
{
    [Route("api/")]
    public class CatalogController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CatalogController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("brand")]
        [ProducesResponseType(typeof(CreateBrandResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CreateBrandResponse>> CreateBrand([FromBody]CreateBrandRequest request, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(
                new CreateBrandCommand(request.Name),
                cancellationToken);

            return result.ToResponse();
        }

        // Update Brand

        // Delete Brand

        // Create Category

        // Update Category

        // Delete Category

        // Create Product

        // Update Product

        // Delete Product

        // Get Product List

        // Get Categories

        // Get Brands
    }
}
