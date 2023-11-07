namespace eShopApp.Shared.Primitives
{
    /// <summary>
    /// 
    /// </summary>
    public class ErrorResponse
    {
        /// <summary>
        /// 
        /// </summary>
        public string ErrorCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="error"></param>
        public static implicit operator ErrorResponse(Error error)
        {

            var response = error.HttpStatusCode switch
            {

                400 => new ErrorResponse()
                {
                    ErrorCode = error.Code,
                    Description = error.Message
                },
                _ => null
            };

            return response;
        }
    }
}
