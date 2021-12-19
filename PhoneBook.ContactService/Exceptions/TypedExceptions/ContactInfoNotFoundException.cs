using PhoneBook.Library.Exceptions;

namespace PhoneBook.ContactService.Exceptions.TypedExceptions
{
    public class ContactInfoNotFoundException : BaseBadRequestException
    {
        public ContactInfoNotFoundException()
        {
            this.ErrorCode = ExceptionCodes.ContactInfoNotFound;
            this.ErrorMessage = "Contact info not found.";
        }
    }
}
