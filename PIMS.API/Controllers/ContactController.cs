using Microsoft.AspNetCore.Mvc;
using PIMS.Application.Dtos;
using PIMS.Application.Interfaces;
using PIMS.Application.Mappings;
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
        [HttpPost("create")]
        public async Task<IActionResult> AddAsync([FromBody] ContactDto contact, CancellationToken cancellationToken)
        {
            try
            {
                Contact req = new Contact
                {
                    FirstName = contact.FirstName,
                    LastName = contact.LastName,
                    EmailAddress = contact.EmailAddress,
                    PhoneNumber = contact.PhoneNumber,
                };

                await _contactService.AddAsync(req, cancellationToken);
                return Ok(req);
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

        [HttpPost("create/bulk")]
        public async Task<IActionResult> AddAsync([FromBody] IEnumerable<ContactDto> contacts, CancellationToken cancellationToken)
        {
            // Check if the input data is null or empty
            if (contacts == null || !contacts.Any())
            {
                return BadRequest("No contacts provided.");
            }

            try
            {
                // Map the ContactDto objects to entities
                var entities = ContactMapper.ToEntities(contacts);

                // Call the service to add the contacts to the database asynchronously
                await _contactService.AddAsync(entities, cancellationToken).ConfigureAwait(false);

                // Return a NoContent (204) response indicating success with no content
                return Ok(contacts);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred. " + ex.Message);
            }
        }

        [HttpGet("get")]
        public async Task<IActionResult> GetAllAsync([FromQuery] int pageNumber, [FromQuery] int pageSize, [FromQuery] string? filter, CancellationToken cancellationToken)
        {
            try
            {
                filter = filter == null ? string.Empty : filter;
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

        [HttpGet("get/{id}")]
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
        [HttpPut("update")]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateContactDto contact, CancellationToken cancellationToken)
        {
            try
            {
                Contact req = new Contact
                {
                    FirstName = contact.FirstName,
                    LastName = contact.LastName,
                    EmailAddress = contact.EmailAddress,
                    PhoneNumber = contact.PhoneNumber,
                    Id = contact.Id,
                };

                await _contactService.UpdateAsync(req, cancellationToken);
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
        [HttpPut("update/bulk")]
        public async Task<IActionResult> UpdateAsync([FromBody] IEnumerable<UpdateContactDto> contacts, CancellationToken cancellationToken)
        {
            try
            {
               var req = ContactMapper.ToEntities(contacts);

                await _contactService.UpdateAsync(req, cancellationToken);
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
