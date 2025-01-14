using Microsoft.AspNetCore.Mvc;
using PIMS.Application.Dtos;
using PIMS.Application.Interfaces;
using PIMS.Domain.Entities;

namespace PIMS.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PropertyController : Controller
    {
        private readonly IPropertyService _propertyService;

        public PropertyController(IPropertyService propertyService)
        {
            _propertyService = propertyService;
        }
        [HttpPost]
        public async Task<IActionResult> AddProperty([FromBody] PropertyDto propertyDto
            ,CancellationToken cancellationToken)
        {
            if (propertyDto == null)
            {
                return BadRequest("Invalid data.");
            }

            var property = new Property
            {
                Name = propertyDto.Name,
                Address = propertyDto.Address,
                Price = propertyDto.Price,
                OwnerId = propertyDto.OwnerId,
            };

            await _propertyService.AddAsync(property,cancellationToken);

            return CreatedAtAction(nameof(AddProperty)
                , new { id = property.Id }, property);
        }
        [HttpGet("FindPropertyByName/{name}")]
        public async Task<IActionResult> FindPropertyByName(string name
            ,CancellationToken cancellationToken)
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest("Property name cannot be null or empty.");
            }

            try
            {
                // Call service to retrieve the property
                var property = await _propertyService.GetByNameAsync(name,cancellationToken);

                // Handle not found case
                if (property == null)
                {
                    return NotFound($"Property with name '{name}' was not found.");
                }

                // Return the found property
                return Ok(property);
            }
            catch (Exception ex)
            {
                // Return a generic error message
                return StatusCode(500, "An error occurred while processing your request. Please try again later.");
            }
        }

        [HttpPut("UpdatePropertyByName/{name}")]
        public async Task<IActionResult> UpdateProperty(string name,decimal price, [FromBody] PropertyDto propertyDto
            ,CancellationToken cancellationToken)
        {
            if (propertyDto == null)
            {
                return BadRequest("Invalid data.");
            }

            var property = await _propertyService.GetByNameAndPriceAsync(name, price, cancellationToken );
            if (property == null)
            {
                return NotFound("Property not found.");
            }

            // Update data in record
            property.Name = propertyDto.Name;
            property.Address = propertyDto.Address;
            property.Price = propertyDto.Price;
            property.OwnerId = propertyDto.OwnerId;

            await _propertyService.UpdateAsync(property,cancellationToken);

            return NoContent(); // 204 - success not return data
        }
        [HttpDelete("DeletePropertyByName/{name}")]
        public async Task<IActionResult> DeleteProperty(string name,decimal price,CancellationToken cancellationToken)
        {
            var property = await _propertyService.GetByNameAsync(name,cancellationToken);
            if (property == null)
            {
                return NotFound("Property not found.");
            }

            await _propertyService.DeleteAsync(name,price,cancellationToken);

            return NoContent(); // 204 - success not return data
        }
    }
}
