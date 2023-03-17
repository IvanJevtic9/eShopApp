using Microsoft.AspNetCore.Mvc;
using eShopApp.Catalog.Domain.Shared;
using eShopApp.Catalog.Application.Models;

namespace eShopApp.Catalog.App.Extensions
{
    public static class ControllerExtensions
    {
        public static ActionResult<TResult> ToResponse<TResult>(this Result<TResult> result)
        {
            if (result.IsSuccess)
            {
                return new OkObjectResult(result.GetValue());
            }

            ErrorResponse errorResponse = result.GetError();

            return new ObjectResult(errorResponse)
            {
                StatusCode = StatusCodes.Status400BadRequest
            };
        }
    }
}
