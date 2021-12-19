using PhoneBook.Library.Exceptions;

namespace PhoneBook.ReportService.Exceptions.TypedExceptions
{
    public class RabbitMqUnavailableException : BaseServiceUnavailableException
    {
        public RabbitMqUnavailableException()
        {
            this.ErrorCode = ExceptionCodes.ServiceUnavailable;
            this.ErrorMessage = "Service unavailable.";
        }
    }
}
