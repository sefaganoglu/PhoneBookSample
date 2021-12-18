using ContactService.DAL;
using ContactService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhoneBook.Library;
using PhoneBook.Library.Dto.Request;
using PhoneBook.Library.Dto.Response;

namespace ContactService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactInfoController : BaseController
    {
        private readonly ContactDbContext _context;

        public ContactInfoController(ContactDbContext context)
        {
            _context = context;
        }
        
        [HttpGet("[action]")]
        [ProducesResponseType(typeof(List<ResContactInfoReportDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<ResContactInfoReportDto>>> Report()
        {
            var report = await _context.ContactInfos.Include(p => p.Person)
                                        .GroupBy(p => p.Location)
                                        .Select(p => new ResContactInfoReportDto() { 
                                            Location = p.Key, 
                                            PersonCount = p.Select(q => q.PersonId).Distinct().Count(),
                                            PhoneCount = p.Count()
                                        })
                                        .OrderBy(p => p.Location)
                                        .ToListAsync();

            return Ok(report);
        }

        [HttpPost("[action]")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Guid>> Post([FromBody] ReqContactInfoPostDto dto)
        {
            var person = _context.People.Where(p => p.Id == dto.PersonId).FirstOrDefault();
            if (person == null)
            {
                return BadRequest("Person not found.");
            }

            var contactInfo = new ContactInfo()
            {
                Id = Guid.NewGuid(),
                PersonId = dto.PersonId,
                Phone = dto.Phone,
                Email = dto.Email,
                Location = dto.Location
            };

            await _context.ContactInfos.AddAsync(contactInfo);
            _context.Entry(contactInfo).State = EntityState.Added;
            await _context.SaveChangesAsync();

            return Ok(contactInfo.Id);
        }

        [HttpPut("[action]")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Guid>> Put([FromBody] ReqContactInfoPutDto dto)
        {
            var person = _context.People.Where(p => p.Id == dto.PersonId).FirstOrDefault();
            if (person == null)
            {
                return BadRequest("Person not found.");
            }

            var contactInfo = _context.ContactInfos.Where(p => p.Id == dto.Id).FirstOrDefault();
            if (contactInfo == null)
            {
                return BadRequest("Contact info not found.");
            }

            contactInfo.PersonId = dto.PersonId;
            contactInfo.Phone = dto.Phone;
            contactInfo.Email = dto.Email;
            contactInfo.Location = dto.Location;

            _context.Entry(contactInfo).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(contactInfo.Id);
        }

        [HttpDelete("[action]")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<bool>> Delete(Guid contactInfoId)
        {
            var contactInfo = _context.ContactInfos.Where(p => p.Id == contactInfoId).FirstOrDefault();
            if (contactInfo == null)
            {
                return BadRequest("Contact info not found.");
            }

            _context.Entry(contactInfo).State = EntityState.Deleted;
            await _context.SaveChangesAsync();

            return Ok(true);
        }
    }
}
