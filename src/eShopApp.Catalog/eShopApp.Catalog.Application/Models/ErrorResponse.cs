using eShopApp.Catalog.Domain.Shared;

namespace eShopApp.Catalog.Application.Models
{
    public class ErrorResponse
    {
        public string ErrorCode { get; set; }
        public string Description { get; set; }

        public static implicit operator ErrorResponse(Error error)
        {
            return new ErrorResponse()
            {
                ErrorCode = error.Code,
                Description = error.Message
            };
        }
    }
}
