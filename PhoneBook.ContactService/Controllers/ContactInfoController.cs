using ContactService.DAL;
using ContactService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhoneBook.Library;
using PhoneBook.Library.Dto.Request;

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

        [HttpPost("[action]")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Guid>> Post([FromBody] ReqContactInfoPostDto dto)
        {
            var person = _context.People.Where(p => p.Id == dto.PersonId).FirstOrDefault();
            if (person == null)
            {
                return BadRequest("Person is not found.");
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
                return BadRequest("Person is not found.");
            }

            var contactInfo = _context.ContactInfos.Where(p => p.Id == dto.Id).FirstOrDefault();
            if (contactInfo == null)
            {
                return BadRequest("Contact info is not found.");
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
        public async Task<ActionResult<Guid>> Delete(Guid contactInfoId)
        {
            var contactInfo = _context.ContactInfos.Where(p => p.Id == contactInfoId).FirstOrDefault();
            if (contactInfo == null)
            {
                return BadRequest("Contact info is not found.");
            }

            _context.Entry(contactInfo).State = EntityState.Deleted;
            await _context.SaveChangesAsync();

            return Ok(true);
        }
    }
}
