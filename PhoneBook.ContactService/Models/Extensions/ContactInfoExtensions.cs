using ContactService.Models;
using PhoneBook.Library.Dto.Response;

namespace PhoneBook.ContactService.Models.Extensions
{
    public static class ContactInfoExtensions
    {
        public static ResContactInfoGetDto ToDto(this ContactInfo contactInfo)
        {
            var dto = new ResContactInfoGetDto()
            {
                Id = contactInfo.Id,
                Phone = contactInfo.Phone,
                Email = contactInfo.Email,
                Location = contactInfo.Location
            };

            return dto;
        }
    }
}
