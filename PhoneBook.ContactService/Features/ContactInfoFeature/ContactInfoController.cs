using ContactService.DAL;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PhoneBook.ContactService.Features.ContactInfoFeature.Commands;
using PhoneBook.ContactService.Features.ContactInfoFeature.Queries;
using PhoneBook.Library;
using PhoneBook.Library.Dto.Response;
using PhoneBook.Library.Middlewares;

namespace ContactService.Features.ContactInfoFeature
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactInfoController : BaseController
    {
        private readonly IMediator _mediator;

        public ContactInfoController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpGet("[action]")]
        [ProducesResponseType(typeof(List<ResContactInfoReportDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionHttpResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<ResContactInfoReportDto>>> Report()
        {
            return await _mediator.Send(new ContactInfoReportQuery());
        }

        [HttpPost("[action]")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionHttpResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Guid>> Post([FromBody] ContactInfoPostCommand dto)
        {
            return await _mediator.Send(dto);
        }

        [HttpPut("[action]")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionHttpResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Guid>> Put([FromBody] ContactInfoPutCommand dto)
        {
            return await _mediator.Send(dto);
        }

        [HttpDelete("[action]")]
        [ProducesResponseType(typeof(Unit), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionHttpResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Unit>> Delete([FromBody] ContactInfoDeleteCommand dto)
        {
            return await _mediator.Send(dto);
        }
    }
}
