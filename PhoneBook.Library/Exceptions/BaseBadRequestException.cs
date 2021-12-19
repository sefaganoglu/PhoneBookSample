namespace PhoneBook.Library.Exceptions
{
    public class BaseBadRequestException : BaseException
    {
        public BaseBadRequestException() : base("BadRequest")
        {

        }

        public BaseBadRequestException(string errorCode, string errorMessage) : base("BadRequest", errorCode, errorMessage)
        {

        }
    }
}
