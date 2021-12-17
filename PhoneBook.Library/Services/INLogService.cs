using PhoneBook.Library.Dto.Response;

namespace PhoneBook.Library.Services
{
    public interface INLogService
    {
        bool SaveException(ResSaveExceptionDto resSaveExceptionDto);
    }
}
