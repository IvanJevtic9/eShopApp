using System.Text.Json;
using eShopApp.Catalog.Application.Models;

namespace eShopApp.Catalog.App.Middelware
{
    public sealed class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            _logger.LogError("Exception:{@Exception}\nInnerException:{@InnerException}", exception, exception.InnerException);

            var errorResponse = exception switch
            {
                _ => HandleGlobalException(context, exception),
            };

            var responseJson = JsonSerializer.Serialize(errorResponse);

            await context.Response.WriteAsync(responseJson);
        }

        private ErrorResponse HandleGlobalException(HttpContext context, Exception exception)
        {
            var response = new ErrorResponse()
            {
                ErrorCode = "Unknown",
                Description = exception.Message
            };

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            return response;
        }
    }
}
