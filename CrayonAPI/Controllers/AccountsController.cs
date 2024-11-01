using CrayonAPI.Entities;
using CrayonAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CrayonAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet("{customerId}")]
        public async Task<ActionResult<IEnumerable<Account>>> GetAccounts(int customerId)
        {
            var accounts = await _accountService.GetAccounts(customerId);

            if (accounts == null || !accounts.Any())
            {
                return NotFound($"No accounts found for customer with ID {customerId}.");
            }

            return Ok(accounts);
        }
    }
}
