using ContactService.DAL;
using ContactService.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PhoneBook.ContactService.Exceptions.TypedExceptions;
using System.ComponentModel.DataAnnotations;

namespace PhoneBook.ContactService.Features.PersonFeature.Commands
{
    public class PersonPostCommand : IRequest<Guid>
    {
        [MinLength(1)]
        public string Name { get; set; }

        [MinLength(1)]
        public string Surname { get; set; }

        public string? Company { get; set; }
    }

    public class PersonPostCommandHandler : IRequestHandler<PersonPostCommand, Guid>
    {
        private readonly ContactDbContext _context;

        public PersonPostCommandHandler(ContactDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> Handle(PersonPostCommand request, CancellationToken cancellationToken)
        {
            var person = new Person()
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Surname = request.Surname,
                Company = request.Company
            };

            await _context.People.AddAsync(person, cancellationToken);
            _context.Entry(person).State = EntityState.Added;
            await _context.SaveChangesAsync(cancellationToken);

            return person.Id;
        }
    }
}
