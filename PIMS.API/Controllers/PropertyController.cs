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
        public async Task<IActionResult> AddAsync([FromBody] Property entity, CancellationToken cancellationToken)
        {
            try
            {
                await _propertyService.AddAsync(entity, cancellationToken);
                return CreatedAtAction(nameof(GetByIdAsync), new { id = entity.Id }, entity);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpPost("bulk")]
        public async Task<IActionResult> AddAsync([FromBody] IEnumerable<Property> entities, CancellationToken cancellationToken)
        {
            try
            {
                await _propertyService.AddAsync(entities, cancellationToken);
                return Ok(entities);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery] int pageNumber, [FromQuery] int pageSize, [FromQuery] string filter, CancellationToken cancellationToken)
        {
            try
            {
                var properties = await _propertyService.GetAllAsync(pageNumber, pageSize, filter, cancellationToken);
                return Ok(properties);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                var property = await _propertyService.GetByIdAsync(id, cancellationToken);
                return Ok(property);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] Property entity, CancellationToken cancellationToken)
        {
            try
            {
                await _propertyService.UpdateAsync(entity, cancellationToken);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpPut("bulk")]
        public async Task<IActionResult> UpdateAsync([FromBody] IEnumerable<Property> entities, CancellationToken cancellationToken)
        {
            try
            {
                await _propertyService.UpdateAsync(entities, cancellationToken);
                return Ok(entities);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "An unexpected error occurred.");
            }
        }
    }
}
