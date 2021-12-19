using ContactService.DAL;
using ContactService.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PhoneBook.ContactService.Exceptions.TypedExceptions;

namespace PhoneBook.ContactService.Features.ContactInfoFeature.Commands
{
    public class ContactInfoPutCommand : IRequest<Guid>
    {
        public Guid Id { get; set; }

        public Guid PersonId { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string Location { get; set; }
    }

    public class ContactInfoPutCommandHandler : IRequestHandler<ContactInfoPutCommand, Guid>
    {
        private readonly ContactDbContext _context;

        public ContactInfoPutCommandHandler(ContactDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> Handle(ContactInfoPutCommand request, CancellationToken cancellationToken)
        {
            var person = await _context.People.Where(p => p.Id == request.PersonId).FirstOrDefaultAsync(cancellationToken);
            if (person == null)
            {
                throw new PersonNotFoundException();
            }

            var contactInfo = await _context.ContactInfos.Where(p => p.Id == request.Id).FirstOrDefaultAsync(cancellationToken);
            if (contactInfo == null)
            {
                throw new ContactInfoNotFoundException();
            }

            contactInfo.PersonId = request.PersonId;
            contactInfo.Phone = request.Phone;
            contactInfo.Email = request.Email;
            contactInfo.Location = request.Location;

            _context.Entry(contactInfo).State = EntityState.Modified;
            await _context.SaveChangesAsync(cancellationToken);

            return contactInfo.Id;
        }
    }
}
