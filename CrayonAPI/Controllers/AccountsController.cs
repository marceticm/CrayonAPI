using CrayonAPI.DTOs;
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
        public async Task<ActionResult<IEnumerable<AccountResponseDto>>> GetAccounts(int customerId)
        {
            var accounts = await _accountService.GetAccounts(customerId);

            if (accounts == null || !accounts.Any())
            {
                return NotFound($"No accounts found for customer with ID {customerId}.");
            }

            return Ok(accounts);
        }

        [HttpPost]
        public async Task<ActionResult<Account>> CreateAccount([FromBody] AccountCreateDto accountDto)
        {
            if (accountDto == null)
            {
                return BadRequest("Account data is invalid.");
            }

            try
            {
                var newAccount = await _accountService.CreateAccount(accountDto);
                return CreatedAtAction(nameof(CreateAccount), new { id = newAccount.Id }, newAccount);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
