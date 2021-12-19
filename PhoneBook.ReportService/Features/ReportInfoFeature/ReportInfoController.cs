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
using MediatR;
using PhoneBook.Library.Middlewares;
using PhoneBook.ReportService.Features.ReportInfoFeature.Queries;
using PhoneBook.ReportService.Features.ReportInfoFeature.Commands;
using PhoneBook.ReportService.Features.ReportInfoFeature.Queries;

namespace ReportService.Features.ReportInfoFeature
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportInfoController : BaseController
    {
        private IMediator _mediator;

        public ReportInfoController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("[action]")]
        [ProducesResponseType(typeof(List<ResReportInfoListDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionHttpResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<ResReportInfoListDto>>> List()
        {
            return await _mediator.Send(new ReportInfoListQuery());
        }

        [HttpPost("[action]")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionHttpResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ExceptionHttpResponse), StatusCodes.Status503ServiceUnavailable)]
        public async Task<ActionResult<Guid>> Post()
        {
            return await _mediator.Send(new ReportInfoPostCommand());
        }

        [HttpGet("[action]")]
        [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionHttpResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ExceptionHttpResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ExceptionHttpResponse), StatusCodes.Status503ServiceUnavailable)]
        public async Task<ActionResult> Download([FromQuery] ReportInfoDownloadQuery dto)
        {
            var result = await _mediator.Send(dto);
            var bytes = await System.IO.File.ReadAllBytesAsync(result.FilePath);
            return File(bytes, result.ContentType, Path.GetFileName(result.FilePath));
        }
    }
}
