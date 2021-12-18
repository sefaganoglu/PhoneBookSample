using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using PhoneBook.Library;
using PhoneBook.Library.Dto.Response;
using PhoneBook.ReportService.DAL;
using PhoneBook.ReportService.Services;
using PhoneBook.ReportService.Models;
using PhoneBook.ReportService.Models.Extensions;

namespace ReportService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportInfoController : BaseController
    {
        private readonly ReportDbContext _context;
        private RabbitMQService _rabbitMQService;

        public ReportInfoController(ReportDbContext context, RabbitMQService rabbitMQService)
        {
            _context = context;
            _rabbitMQService = rabbitMQService;
        }

        [HttpGet("[action]")]
        [ProducesResponseType(typeof(List<ResReportInfoListDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<ResReportInfoListDto>>> List()
        {
            var reportInfos = await _context.ReportInfos.OrderByDescending(p => p.RequestDate).ToListAsync();

            var dto = new List<ResReportInfoListDto>();

            foreach (var reportInfo in reportInfos)
            {
                dto.Add(reportInfo.ToListDto());
            }

            return Ok(dto);
        }

        [HttpPost("[action]")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Guid>> Post()
        {
            await _context.Database.BeginTransactionAsync();
            try
            {
                var reportInfo = new ReportInfo()
                {
                    Id = Guid.NewGuid(),
                    RequestDate = DateTime.UtcNow,
                    Status = "Waiting"
                };

                await _context.ReportInfos.AddAsync(reportInfo);
                _context.Entry(reportInfo).State = EntityState.Added;
                await _context.SaveChangesAsync();

                _rabbitMQService.Connect();
                _rabbitMQService.WriteToCreateReportQueue(reportInfo.Id);

                await _context.Database.CommitTransactionAsync();

                return Ok(reportInfo.Id);
            }
            catch (Exception)
            {
                await _context.Database.RollbackTransactionAsync();

                throw;
            }
        }

        [HttpGet("[action]")]
        [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Download(Guid reportInfoId)
        {
            var reportInfo = await _context.ReportInfos.Where(p => p.Id == reportInfoId).FirstOrDefaultAsync();
            if (reportInfo == null)
            {
                return BadRequest("Report info not found.");
            }

            if (reportInfo.Status != "Ready")
            {
                return BadRequest("This report is not yet ready.");
            }

            if (!System.IO.File.Exists(reportInfo.FilePath))
            {
                return BadRequest("File not found.");
            }

            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(reportInfo.FilePath, out var contentType))
            {
                contentType = "text/csv";
            }

            var bytes = await System.IO.File.ReadAllBytesAsync(reportInfo.FilePath);
            return File(bytes, contentType, Path.GetFileName(reportInfo.FilePath));
        }
    }
}
