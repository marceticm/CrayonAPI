using CrayonAPI.Entities;

namespace CrayonAPI.Interfaces
{
    public interface IAccountService
    {
        Task<IEnumerable<Account>> GetAccounts(int customerId);
    }
}
