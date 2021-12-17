using PhoneBook.Library.Dto;

namespace PhoneBook.Library.Services
{
    public interface INLogService
    {
        bool SaveException(ResSaveExceptionDto resSaveExceptionDto);
    }
}
