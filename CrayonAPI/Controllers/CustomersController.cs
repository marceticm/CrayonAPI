using CrayonAPI.DTOs;
using CrayonAPI.Entities;
using CrayonAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CrayonAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService)
        {
                _customerService = customerService;
        }

        [HttpPost] // TO DO: Authorize this endpoint so only admins can create customers
        public async Task<ActionResult<Customer>> CreateCustomer([FromBody] CustomerCreateDto customerDto)
        {
            if (customerDto == null)
            {
                return BadRequest("Customer data is invalid.");
            }

            var createdCustomer = await _customerService.CreateCustomer(customerDto);
            return CreatedAtAction(nameof(CreateCustomer), new { id = createdCustomer.Id }, createdCustomer);
        }
    }
}
