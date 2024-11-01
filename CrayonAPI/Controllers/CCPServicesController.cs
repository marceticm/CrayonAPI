using CrayonAPI.Entities;
using CrayonAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CrayonAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CCPServicesController : ControllerBase
    {
        private readonly ICCPService _ccpService;

        public CCPServicesController(ICCPService ccpService)
        {
            _ccpService = ccpService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Service>>> GetAvailableServices()
        {
            var services = await _ccpService.GetAvailableServices();
            return Ok(services);
        }
    }
}
