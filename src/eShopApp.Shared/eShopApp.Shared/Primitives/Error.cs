namespace eShopApp.Shared.Primitives
{
    /// <summary>
    /// 
    /// </summary>
    public class Error : ValueObject
    {
        /// <summary>
        /// 
        /// </summary>
        public string Code { get; init; }

        /// <summary>
        /// 
        /// </summary>
        public string Message { get; init; }

        /// <summary>
        /// 
        /// </summary>
        public int HttpStatusCode { get; init; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        /// <param name="httpStatusCode"></param>
        public Error(string code, string message, int httpStatusCode = 400)
        {
            Code = code;
            Message = message;
            HttpStatusCode = httpStatusCode;
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly Error Empty =
            new Error(
                string.Empty,
                string.Empty);

        /// <summary>
        /// 
        /// </summary>
        public static readonly Error Forbid =
            new Error(
                "Forbidden",
                "Forbidden access.",
                403);

        /// <summary>
        /// 
        /// </summary>
        public static readonly Error NotFound =
            new Error(
                "NotFound",
                "Not found.",
                404);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="error"></param>
        public static implicit operator string(Error error)
        {
            return error.Code;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Code;
            yield return Message;
            yield return HttpStatusCode;
        }
    }
}
