using PhoneBook.Library.Exceptions;

namespace PhoneBook.ContactService.Exceptions.TypedExceptions
{
    public class PersonNotFoundException : BaseBadRequestException
    {
        public PersonNotFoundException()
        {
            this.ErrorCode = ExceptionCodes.PersonNotFound;
            this.ErrorMessage = "Person not found.";
        }
    }
}
