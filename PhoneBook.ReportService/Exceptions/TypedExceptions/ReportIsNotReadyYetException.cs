using PhoneBook.Library.Exceptions;

namespace PhoneBook.ReportService.Exceptions.TypedExceptions
{
    public class ReportIsNotReadyYetException : BaseServiceUnavailableException
    {
        public ReportIsNotReadyYetException()
        {
            this.ErrorCode = ExceptionCodes.ServiceUnavailable;
            this.ErrorMessage = "Report is not ready yet.";
        }
    }
}
