using MediatR;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using PhoneBook.ReportService.DAL;
using PhoneBook.ReportService.Exceptions.TypedExceptions;
using PhoneBook.ReportService.Services;

namespace PhoneBook.ReportService.Features.ReportInfoFeature.Queries
{
    public class ReportInfoDownloadQueryResult
    {
        public string FilePath { get; set; }
        public string ContentType { get; set; }
    }

    public class ReportInfoDownloadQuery : IRequest<ReportInfoDownloadQueryResult>
    {
        public Guid Id { get; set; }
    }

    public class ReportInfoDownloadQueryHandler : IRequestHandler<ReportInfoDownloadQuery, ReportInfoDownloadQueryResult>
    {
        private readonly ReportDbContext _context;
        private readonly IFileService _fileService;

        public ReportInfoDownloadQueryHandler(ReportDbContext context, IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }

        public async Task<ReportInfoDownloadQueryResult> Handle(ReportInfoDownloadQuery request, CancellationToken cancellationToken)
        {
            var reportInfo = await _context.ReportInfos.Where(p => p.Id == request.Id).FirstOrDefaultAsync();
            if (reportInfo == null)
            {
                throw new ReportInfoNotFoundException();
            }

            if (reportInfo.Status != "Ready")
            {
                throw new ReportIsNotReadyYetException();
            }

            if (!_fileService.Exists(reportInfo.FilePath))
            {
                throw new ReportFileNotFoundException();
            }

            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(reportInfo.FilePath, out var contentType))
            {
                contentType = "text/csv";
            }

            return new ReportInfoDownloadQueryResult()
            {
                FilePath = reportInfo.FilePath,
                ContentType = contentType
            };
        }
    }
}
