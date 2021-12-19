using PhoneBook.ReportService.DAL;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PhoneBook.ReportService.Models.Extensions;
using PhoneBook.Library.Dto.Response;

namespace PhoneBook.ReportService.Features.ReportInfoFeature.Queries
{
    public class ReportInfoListQuery : IRequest<List<ResReportInfoListDto>>
    {

    }

    public class ReportInfoListQueryHandler : IRequestHandler<ReportInfoListQuery, List<ResReportInfoListDto>>
    {
        private readonly ReportDbContext _context;

        public ReportInfoListQueryHandler(ReportDbContext context)
        {
            _context = context;
        }

        public async Task<List<ResReportInfoListDto>> Handle(ReportInfoListQuery request, CancellationToken cancellationToken)
        {
            var reportInfos = await _context.ReportInfos.OrderByDescending(p => p.RequestDate).ToListAsync(cancellationToken);

            var dto = new List<ResReportInfoListDto>();

            foreach (var reportInfo in reportInfos)
            {
                dto.Add(reportInfo.ToListDto());
            }

            return dto;
        }
    }
}
