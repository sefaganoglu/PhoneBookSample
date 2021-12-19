using ContactService.DAL;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PhoneBook.Library.Dto.Response;

namespace PhoneBook.ContactService.Features.ContactInfoFeature.Queries
{
    public class ContactInfoReportQuery : IRequest<List<ResContactInfoReportDto>>
    {

    }

    public class ContactInfoReportQueryHandler : IRequestHandler<ContactInfoReportQuery, List<ResContactInfoReportDto>>
    {
        private readonly ContactDbContext _context;

        public ContactInfoReportQueryHandler(ContactDbContext context)
        {
            _context = context;
        }

        public async Task<List<ResContactInfoReportDto>> Handle(ContactInfoReportQuery request, CancellationToken cancellationToken)
        {
            var report = await _context.ContactInfos.Include(p => p.Person)
                                                    .GroupBy(p => p.Location)
                                                    .Select(p => new ResContactInfoReportDto()
                                                    {
                                                        Location = p.Key,
                                                        PersonCount = p.Select(q => q.PersonId).Distinct().Count(),
                                                        PhoneCount = p.Count()
                                                    })
                                                    .OrderBy(p => p.Location)
                                                    .ToListAsync();

            return report;
        }
    }
}
