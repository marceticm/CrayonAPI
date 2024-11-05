using CrayonAPI.DTOs;
using CrayonAPI.Entities;
using CrayonAPI.Interfaces;

namespace CrayonAPI.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ICustomerRepository _customerRepository;

        public AccountService(IAccountRepository accountRepository, ICustomerRepository customerRepository)
        {
            _accountRepository = accountRepository;
            _customerRepository = customerRepository;
        }

        public async Task<IEnumerable<AccountResponseDto>> GetAccounts(int customerId)
        {
            var accounts = await _accountRepository.GetAccounts(customerId);

            return accounts.Select(account => new AccountResponseDto
            {
                Id = account.Id,
                CustomerId = account.CustomerId,
                AccountName = account.AccountName,
                CustomerName = account.Customer.Name
            });
        }

        public async Task<AccountResponseDto> CreateAccount(AccountCreateDto accountDto)
        {
            var customer = await _customerRepository.GetCustomer(accountDto.CustomerId);
            if (customer == null)
            {
                throw new ArgumentException("Invalid CustomerId");
            }

            var account = new Account
            {
                CustomerId = accountDto.CustomerId,
                AccountName = accountDto.AccountName,
                Customer = customer
            };

            var createdAccount = await _accountRepository.AddAccount(account);

            return new AccountResponseDto
            {
                Id = createdAccount.Id,
                CustomerId = customer.Id,
                AccountName = accountDto.AccountName,
                CustomerName = customer.Name
            };
        }
    }
}
