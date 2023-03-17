namespace eShopApp.Catalog.Domain.Shared
{
    public class Result<T>
    {
        private readonly T _value;
        private readonly Error _error;

        public bool IsSuccess { get; private set; }

        public bool IsFailure { get; private set; }

        public Result(Error error)
        {
            _error = error;

            IsFailure = true;
        }

        public Result(T value)
        {
            _value = value;

            IsSuccess = true;
        }

        public Error GetError()
        {
            return IsFailure ? _error : null;
        }

        public T GetValue()
        {
            return IsSuccess ? _value : default;
        }
    }
}
