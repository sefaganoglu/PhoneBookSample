using ContactService.DAL;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PhoneBook.ContactService.Models.Extensions;
using PhoneBook.Library.Dto.Response;

namespace PhoneBook.ContactService.Features.ProductFeature.Queries
{
    public class PersonListQuery : IRequest<List<ResPersonListDto>>
    {

    }

    public class PersonListQueryHandler : IRequestHandler<PersonListQuery, List<ResPersonListDto>>
    {
        private readonly ContactDbContext _context;

        public PersonListQueryHandler(ContactDbContext context)
        {
            _context = context;
        }

        public async Task<List<ResPersonListDto>> Handle(PersonListQuery request, CancellationToken cancellationToken)
        {
            var persons = await _context.People
                                        .OrderBy(p => p.Name)
                                        .ThenByDescending(p => p.Surname)
                                        .ToListAsync(cancellationToken);

            var dto = new List<ResPersonListDto>();

            foreach (var person in persons)
            {
                dto.Add(person.ToListDto());
            }

            return dto;
        }
    }
}
