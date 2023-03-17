namespace eShopApp.Catalog.Domain.Shared
{
    public class Error : IEquatable<Error>
    {
        public string Code { get; set; }
        public string Message { get; set; }

        public Error(string code, string message)
        {
            Code = code;
            Message = message;
        }

        public static readonly Error None = new Error(string.Empty, string.Empty);
        public static readonly Error NullValue = new Error("Error.NullValue", "The specified result value is null.");

        public static implicit operator string(Error error) => error.Code;

        public static bool operator ==(Error error, Error otherError)
        {
            if (error is null && otherError is null)
                return true;

            if (error is null || otherError is null)
                return false;

            return error.Equals(otherError);
        }

        public static bool operator !=(Error error, Error otherError)
        {
            return !(error == otherError);
        }

        public override bool Equals(object obj)
        {
            if (obj is null)
                return false;

            if (obj is not Error err) 
                return false;

            return Code == err.Code &&
                Message == err.Message;
        }

        public bool Equals(Error other)
        {
            return Equals(other as object);
        }

        public override int GetHashCode()
        {
            return Code.GetHashCode() * 41;
        }
    }
}
