using ContactService.DAL;
using ContactService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhoneBook.ContactService.Models.Extensions;
using PhoneBook.Library;
using PhoneBook.Library.Dto.Request;
using PhoneBook.Library.Dto.Response;

namespace ContactService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : BaseController
    {
        private readonly ContactDbContext _context;

        public PersonController(ContactDbContext context)
        {
            _context = context;
        }

        [HttpGet("[action]")]
        [ProducesResponseType(typeof(List<ResPersonListDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<ResPersonListDto>>> List()
        {
            var persons = await _context.People
                                        .OrderBy(p => p.Name )
                                        .ThenByDescending(p => p.Surname)
                                        .ToListAsync();

            var dto = new List<ResPersonListDto>();

            foreach (var person in persons)
            {
                dto.Add(person.ToListDto());
            }

            return Ok(dto);
        }

        [HttpGet("[action]")]
        [ProducesResponseType(typeof(ResPersonGetDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ResPersonGetDto>> Get(Guid personId)
        {
            var person = await _context.People.Include(p => p.ContactInfos).Where(p => p.Id == personId).FirstOrDefaultAsync();
            if (person == null)
            {
                return BadRequest("Person not found.");
            }

            return Ok(person.ToGetDto());
        }

        [HttpPost("[action]")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Guid>> Post([FromBody] ReqPersonPostDto dto)
        {
            var person = new Person()
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Surname = dto.Surname,
                Company = dto.Company
            };

            await _context.People.AddAsync(person);
            _context.Entry(person).State = EntityState.Added;
            await _context.SaveChangesAsync();

            return Ok(person.Id);
        }

        [HttpPut("[action]")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Guid>> Put([FromBody] ReqPersonPutDto dto)
        {
            var person = await _context.People.Where(p => p.Id == dto.Id).FirstOrDefaultAsync();
            if (person == null)
            {
                return BadRequest("Person not found.");
            }

            person.Name = dto.Name;
            person.Surname = dto.Surname;
            person.Company = dto.Company;

            _context.Entry(person).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(person.Id);
        }

        [HttpDelete("[action]")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Guid>> Delete(Guid personId)
        {
            var person = await _context.People.Where(p => p.Id == personId).FirstOrDefaultAsync();
            if (person == null)
            {
                return BadRequest("Person not found.");
            }

            _context.Entry(person).State = EntityState.Deleted;
            await _context.SaveChangesAsync();

            return Ok(true);
        }
    }
}
