using CrayonAPI.Entities;

namespace CrayonAPI.Interfaces
{
    public interface IAccountRepository
    {
        Task<IEnumerable<Account>> GetAccounts(int customerId);
    }
}
