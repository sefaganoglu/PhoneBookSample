using ContactService.DAL;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PhoneBook.ContactService.Exceptions.TypedExceptions;
using PhoneBook.ContactService.Models.Extensions;
using PhoneBook.Library.Dto.Response;

namespace PhoneBook.ContactService.Features.ProductFeature.Queries
{
    public class PersonGetQuery : IRequest<ResPersonGetDto>
    {
        public Guid Id { get; set; }
    }

    public class PersonGetQueryHandler : IRequestHandler<PersonGetQuery, ResPersonGetDto>
    {
        private readonly ContactDbContext _context;

        public PersonGetQueryHandler(ContactDbContext context)
        {
            _context = context;
        }

        public async Task<ResPersonGetDto> Handle(PersonGetQuery request, CancellationToken cancellationToken)
        {
            var person = await _context.People.Include(p => p.ContactInfos).Where(p => p.Id == request.Id).FirstOrDefaultAsync(cancellationToken);
            if (person == null)
            {
                throw new PersonNotFoundException();
            }

            return person.ToGetDto();
        }
    }
}
