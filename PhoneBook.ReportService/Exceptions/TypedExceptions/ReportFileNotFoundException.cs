using PhoneBook.Library.Exceptions;

namespace PhoneBook.ReportService.Exceptions.TypedExceptions
{
    public class ReportFileNotFoundException : BaseInternalServerErrorException
    {
        public ReportFileNotFoundException()
        {
            this.ErrorCode = ExceptionCodes.ReportFileNotFound;
            this.ErrorMessage = "Report file not found.";
        }
    }
}
