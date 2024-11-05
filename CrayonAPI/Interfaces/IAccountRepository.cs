using CrayonAPI.Entities;

namespace CrayonAPI.Interfaces
{
    public interface IAccountRepository
    {
        Task<IEnumerable<Account>> GetAccounts(int customerId);
        Task<Account?> GetAccount(int accountId);
        Task<Account> AddAccount(Account account);
    }
}
