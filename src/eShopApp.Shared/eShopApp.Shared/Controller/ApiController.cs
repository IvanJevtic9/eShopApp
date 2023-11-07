using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace eShopApp.Shared.Controller
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public abstract class ApiController : ControllerBase
    {
        protected IMediator _mediator { get; }

        protected ApiController(IMediator mediator)
        {
            _mediator = mediator;
        }
    }
}
