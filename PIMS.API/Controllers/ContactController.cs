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
        public async Task<IActionResult> AddContact([FromBody] ContactDto contactDto
            , CancellationToken cancellationToken)
        {
            if (contactDto == null)
            {
                return BadRequest("Invalid data.");
            }

            var contact = new Contact
            {
                FirstName = contactDto.FirstName,
                LastName = contactDto.LastName,
                EmailAddress = contactDto.EmailAddress,
                PhoneNumber = contactDto.PhoneNumber,
            };

            await _contactService.AddAsync(contact, cancellationToken);

            return CreatedAtAction(nameof(AddContact)
                , new { id = contact.Id }, contact);
        }
        [HttpGet("FindContactByEmail/{email}")]
        public async Task<IActionResult> FindContactByEmail(string email
            , CancellationToken cancellationToken)
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(email))
            {
                return BadRequest("Contact email cannot be null or empty.");
            }

            try
            {
                // Call service to retrieve the contact
                var contact = await _contactService.GetByEmailAsync(email, cancellationToken);

                // Handle not found case
                if (contact == null)
                {
                    return NotFound($"Contact with Email '{email}' was not found.");
                }

                // Return the found property
                return Ok(contact);
            }
            catch (Exception ex)
            {
                // Return a generic error message
                return StatusCode(500, "An error occurred while processing your request. Please try again later.");
            }
        }

        [HttpPut("UpdateContact")]
        public async Task<IActionResult> UpdateContact([FromBody] ContactDto req
            , CancellationToken cancellationToken)
        {
            if (req == null)
            {
                return BadRequest("Invalid data.");
            }

            var res = await _contactService.GetByEmailAsync(req.EmailAddress, cancellationToken);
            if (res == null)
            {
                return NotFound("Contact not found.");
            }

            // Update data in record
            res.FirstName = req.FirstName;
            res.LastName = req.LastName;
            res.EmailAddress = req.EmailAddress;
            res.PhoneNumber = req.PhoneNumber;

            await _contactService.UpdateAsync(res, cancellationToken);

            return NoContent(); // 204 - success not return data
        }
        [HttpDelete("DeleteContact")]
        public async Task<IActionResult> DeleteProperty(string email,CancellationToken cancellationToken)
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(email))
            {
                return BadRequest("Contact email cannot be null or empty.");
            }
            var contact = await _contactService.GetByEmailAsync(email,cancellationToken);
            if (contact == null)
            {
                return NotFound("Contact not found.");
            }

            await _contactService.DeleteAsync(contact.Id,cancellationToken);

            return NoContent(); // 204 - success not return data
        }
    }
}
