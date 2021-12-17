using ContactService.Models;
using PhoneBook.Library.Dto.Response;

namespace PhoneBook.ContactService.Models.Extensions
{
    public static class PersonExtensions
    {
        public static ResPersonGetDto ToGetDto(this Person person)
        {
            var dto = new ResPersonGetDto()
            {
                Id = person.Id,
                Name = person.Name,
                Surname = person.Surname,
                Company = person.Company,
                ContactInfos = new List<ResContactInfoGetDto>()
            };

            foreach (var contactInfo in person.ContactInfos)
            {
                dto.ContactInfos.Add(contactInfo.ToDto());
            }

            return dto;
        }
        public static ResPersonListDto ToListDto(this Person person)
        {
            var dto = new ResPersonListDto()
            {
                Id = person.Id,
                Name = person.Name,
                Surname = person.Surname,
                Company = person.Company
            };

            return dto;
        }
    }
}
