namespace PhoneBook.Library.Exceptions
{
    public abstract class BaseException : Exception
    {
        public BaseException(string errorType)
        {
            this.ErrorType = errorType;
        }

        public BaseException(string errorType, string errorCode, string errorMessage)
        {
            this.ErrorType = errorType;
            this.ErrorCode = errorCode;
            this.ErrorMessage = errorMessage;
        }

        public string ErrorType { get; init; }
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
    }
}
