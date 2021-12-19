using ContactService.DAL;
using ContactService.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PhoneBook.ContactService.Exceptions.TypedExceptions;

namespace PhoneBook.ContactService.Features.PersonFeature.Commands
{
    public class PersonDeleteCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
    }

    public class PersonDeleteCommandHandler : IRequestHandler<PersonDeleteCommand, Unit>
    {
        private readonly ContactDbContext _context;

        public PersonDeleteCommandHandler(ContactDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(PersonDeleteCommand request, CancellationToken cancellationToken)
        {
            var person = await _context.People.Where(p => p.Id == request.Id).FirstOrDefaultAsync(cancellationToken);
            if (person == null)
            {
                throw new PersonNotFoundException();
            }

            _context.Entry(person).State = EntityState.Deleted;
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
