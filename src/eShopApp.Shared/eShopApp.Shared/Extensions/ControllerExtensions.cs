using Microsoft.AspNetCore.Mvc;
using eShopApp.Shared.Primitives;

namespace eShopApp.Shared.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class ControllerExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="result"></param>
        /// <returns></returns>
        public static ActionResult<TResult> ToResponse<TResult>(this Result<TResult> result)
        {
            if (result.IsSuccess)
            {
                return new OkObjectResult(result.GetValue());
            }

            var error = result.GetError();

            return new ObjectResult((ErrorResponse)error)
            {
                StatusCode = error.HttpStatusCode
            };
        }
    }
}
