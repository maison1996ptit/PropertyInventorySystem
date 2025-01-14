using Microsoft.AspNetCore.Mvc;
using PIMS.Application.Dtos;
using PIMS.Application.Interfaces;
using PIMS.Domain.Entities;

namespace PIMS.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ContactController : Controller
    {
        private readonly IContactService _contactService;

        public ContactController(IContactService contactService)
        {
            _contactService = contactService;
        }
        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] Contact contact, CancellationToken cancellationToken)
        {
            try
            {
                await _contactService.AddAsync(contact, cancellationToken);
                return CreatedAtAction(nameof(GetByIdAsync), new { id = contact.Id }, contact);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("bulk")]
        public async Task<IActionResult> AddAsync([FromBody] IEnumerable<Contact> contacts, CancellationToken cancellationToken)
        {
            try
            {
                await _contactService.AddAsync(contacts, cancellationToken);
                return NoContent(); // Return a successful response with no content for bulk create
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery] int pageNumber, [FromQuery] int pageSize, [FromQuery] string filter, CancellationToken cancellationToken)
        {
            try
            {
                var contacts = await _contactService.GetAllAsync(pageNumber, pageSize, filter, cancellationToken);
                return Ok(contacts);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                var contact = await _contactService.GetByIdAsync(id, cancellationToken);
                return Ok(contact);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] Contact contact, CancellationToken cancellationToken)
        {
            try
            {
                await _contactService.UpdateAsync(contact, cancellationToken);
                return NoContent(); // Return a successful response with no content
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPut("bulk")]
        public async Task<IActionResult> UpdateAsync([FromBody] IEnumerable<Contact> contacts, CancellationToken cancellationToken)
        {
            try
            {
                await _contactService.UpdateAsync(contacts, cancellationToken);
                return NoContent(); // Return a successful response with no content
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
