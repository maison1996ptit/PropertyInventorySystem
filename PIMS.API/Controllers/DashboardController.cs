using Microsoft.AspNetCore.Mvc;
using PIMS.Application.Interfaces;

namespace PIMS.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class DashboardController : Controller
    {
        private readonly  IPropertyService _propertyService;

        public DashboardController(IPropertyService propertyService)
        {
            _propertyService = propertyService;
        }
        [HttpGet]
        public async Task<IActionResult> GetDataDashboardAsync([FromQuery] int pageNumber, [FromQuery] int pageSize, [FromQuery] string? filter, CancellationToken cancellationToken)
        {
            try
            {
                filter = filter == null ? string.Empty : filter;
                var properties = await _propertyService.GetDataDashboardAsync(pageNumber, pageSize, filter, cancellationToken);
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
