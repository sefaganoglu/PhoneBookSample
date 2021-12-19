using PhoneBook.Library.Exceptions;

namespace PhoneBook.ReportService.Exceptions.TypedExceptions
{
    public class ReportInfoNotFoundException : BaseBadRequestException
    {
        public ReportInfoNotFoundException()
        {
            this.ErrorCode = ExceptionCodes.ReportInfoNotFound;
            this.ErrorMessage = "Report info not found.";
        }
    }
}
