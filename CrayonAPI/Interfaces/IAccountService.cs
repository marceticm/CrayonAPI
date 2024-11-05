using CrayonAPI.DTOs;
using CrayonAPI.Entities;

namespace CrayonAPI.Interfaces
{
    public interface IAccountService
    {
        Task<IEnumerable<AccountResponseDto>> GetAccounts(int customerId);
        Task<AccountResponseDto> CreateAccount(AccountCreateDto accountDto);
    }
}
