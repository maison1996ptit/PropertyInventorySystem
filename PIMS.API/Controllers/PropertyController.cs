using Microsoft.AspNetCore.Mvc;
using PIMS.Application.Dtos;
using PIMS.Application.Interfaces;
using PIMS.Application.Mappings;
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
        [HttpPost("create")]
        public async Task<IActionResult> AddAsync([FromBody] PropertyDto property, CancellationToken cancellationToken)
        {
            try
            {
                var req =  new Property 
                { 
                    Address = property.Address ,
                    Price = property.Price ,
                    Name = property.Name ,
                };
                await _propertyService.AddAsync(req, cancellationToken);
                return Ok(req);
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

        [HttpPost("create/bulk")]
        public async Task<IActionResult> AddAsync([FromBody] IEnumerable<PropertyDto> properties, CancellationToken cancellationToken)
        {
            try
            {
                var req = PropertyMapper.ToEntities(properties);

                await _propertyService.AddAsync(req, cancellationToken);
                return Ok(req);
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

        [HttpGet("get")]
        public async Task<IActionResult> GetAllAsync([FromQuery] int pageNumber, [FromQuery] int pageSize, [FromQuery] string? filter, CancellationToken cancellationToken)
        {
            try
            {
                filter = filter == null ? string.Empty : filter;
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

        [HttpGet("get/{id}")]
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

        [HttpPut("update")]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdatePropertyDto req, CancellationToken cancellationToken)
        {
            try
            {
                var property = new Property
                {
                    Name = req.Name,
                    Id = req.Id,
                    Price = req.Price,
                    Address = req.Address,
                };

                await _propertyService.UpdateAsync(property, cancellationToken);
                return Ok();
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

        [HttpPut("update/bulk")]
        public async Task<IActionResult> UpdateAsync([FromBody] IEnumerable<UpdatePropertyDto> req, CancellationToken cancellationToken)
        {
            try
            {
                var properties = PropertyMapper.ToEntities(req);

                await _propertyService.UpdateAsync(properties, cancellationToken);
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
    }
}
