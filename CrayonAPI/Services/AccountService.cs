using CrayonAPI.Entities;
using CrayonAPI.Interfaces;

namespace CrayonAPI.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;

        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<IEnumerable<Account>> GetAccounts(int customerId)
        {
            return await _accountRepository.GetAccounts(customerId);
        }
    }
}
