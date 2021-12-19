using ContactService.DAL;
using ContactService.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PhoneBook.ContactService.Exceptions.TypedExceptions;

namespace PhoneBook.ContactService.Features.ContactInfoFeature.Commands
{
    public class ContactInfoPostCommand : IRequest<Guid>
    {
        public Guid PersonId { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string Location { get; set; }
    }

    public class ContactInfoPostCommandHandler : IRequestHandler<ContactInfoPostCommand, Guid>
    {
        private readonly ContactDbContext _context;

        public ContactInfoPostCommandHandler(ContactDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> Handle(ContactInfoPostCommand request, CancellationToken cancellationToken)
        {
            var person = await _context.People.Where(p => p.Id == request.PersonId).FirstOrDefaultAsync(cancellationToken);
            if (person == null)
            {
                throw new PersonNotFoundException();
            }

            var contactInfo = new ContactInfo()
            {
                Id = Guid.NewGuid(),
                PersonId = request.PersonId,
                Phone = request.Phone,
                Email = request.Email,
                Location = request.Location
            };

            await _context.ContactInfos.AddAsync(contactInfo, cancellationToken);
            _context.Entry(contactInfo).State = EntityState.Added;
            await _context.SaveChangesAsync(cancellationToken);

            return contactInfo.Id;
        }
    }
}
