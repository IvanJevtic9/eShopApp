namespace eShopApp.Shared.Primitives
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Result<T>
    {
        private readonly T _value;
        private readonly Error _error;

        /// <summary>
        /// 
        /// </summary>
        public bool IsSuccess { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsFailure { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="err"></param>
        public Result(Error err)
        {
            _error = err;
            IsFailure = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public Result(T value)
        {
            _value = value;
            IsSuccess = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Error GetError()
        {
            return IsFailure ? _error : null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public T GetValue()
        {
            return IsSuccess ? _value : default;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Result<T> Create(T value) => new Result<T>(value);
    }
}
