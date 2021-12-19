using ContactService.DAL;
using ContactService.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhoneBook.ContactService.Features.PersonFeature.Commands;
using PhoneBook.ContactService.Features.ProductFeature.Queries;
using PhoneBook.ContactService.Models.Extensions;
using PhoneBook.Library;
using PhoneBook.Library.Dto.Response;
using PhoneBook.Library.Middlewares;

namespace ContactService.PersonFeature
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : BaseController
    {
        private readonly IMediator _mediator;

        public PersonController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("[action]")]
        [ProducesResponseType(typeof(List<ResPersonListDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionHttpResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<ResPersonListDto>>> List()
        {
            return await _mediator.Send(new PersonListQuery());
        }

        [HttpGet("[action]")]
        [ProducesResponseType(typeof(ResPersonGetDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionHttpResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ResPersonGetDto>> Get([FromQuery] PersonGetQuery dto)
        {
            return await _mediator.Send(dto);
        }

        [HttpPost("[action]")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionHttpResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Guid>> Post([FromBody] PersonPostCommand dto)
        {
            return await _mediator.Send(dto);
        }

        [HttpPut("[action]")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionHttpResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Guid>> Put([FromBody] PersonPutCommand dto)
        {
            return await _mediator.Send(dto);
        }

        [HttpDelete("[action]")]
        [ProducesResponseType(typeof(Unit), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionHttpResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Unit>> Delete(PersonDeleteCommand dto)
        {
            return await _mediator.Send(dto);
        }
    }
}
