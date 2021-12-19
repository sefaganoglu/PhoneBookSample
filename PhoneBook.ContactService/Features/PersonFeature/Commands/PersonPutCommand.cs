using ContactService.DAL;
using ContactService.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PhoneBook.ContactService.Exceptions.TypedExceptions;
using System.ComponentModel.DataAnnotations;

namespace PhoneBook.ContactService.Features.PersonFeature.Commands
{
    public class PersonPutCommand : IRequest<Guid>
    {
        public Guid Id { get; set; }

        [MinLength(1)]
        public string Name { get; set; }

        [MinLength(1)]
        public string Surname { get; set; }

        public string? Company { get; set; }
    }

    public class PersonPutCommandHandler : IRequestHandler<PersonPutCommand, Guid>
    {
        private readonly ContactDbContext _context;

        public PersonPutCommandHandler(ContactDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> Handle(PersonPutCommand request, CancellationToken cancellationToken)
        {
            var person = await _context.People.Where(p => p.Id == request.Id).FirstOrDefaultAsync(cancellationToken);
            if (person == null)
            {
                throw new PersonNotFoundException();
            }

            person.Name = request.Name;
            person.Surname = request.Surname;
            person.Company = request.Company;

            _context.Entry(person).State = EntityState.Modified;
            await _context.SaveChangesAsync(cancellationToken);

            return person.Id;
        }
    }
}
