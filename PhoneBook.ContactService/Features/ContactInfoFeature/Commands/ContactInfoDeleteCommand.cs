using ContactService.DAL;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PhoneBook.ContactService.Exceptions.TypedExceptions;

namespace PhoneBook.ContactService.Features.ContactInfoFeature.Commands
{
    public class ContactInfoDeleteCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
    }

    public class ContactInfoDeleteCommandHandler : IRequestHandler<ContactInfoDeleteCommand>
    {
        private readonly ContactDbContext _context;

        public ContactInfoDeleteCommandHandler(ContactDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(ContactInfoDeleteCommand request, CancellationToken cancellationToken)
        {
            var contactInfo = await _context.ContactInfos.Where(p => p.Id == request.Id).FirstOrDefaultAsync(cancellationToken);
            if (contactInfo == null)
            {
                throw new ContactInfoNotFoundException();
            }

            _context.Entry(contactInfo).State = EntityState.Deleted;
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
