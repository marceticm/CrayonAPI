using CrayonAPI.Entities;
using CrayonAPI.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CrayonAPI.Data
{
    public class AccountRepository : IAccountRepository
    {
        private readonly CrayonDbContext _context;

        public AccountRepository(CrayonDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Account>> GetAccounts(int customerId)
        {
            return await _context.Accounts
                .Where(x => x.CustomerId == customerId)
                .ToListAsync();
        }
    }
}
