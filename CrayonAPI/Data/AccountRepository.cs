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
                .Include(x => x.Customer)
                .ToListAsync();
        }

        public async Task<Account?> GetAccount(int accountId)
        {
            return await _context.Accounts
                .FirstOrDefaultAsync(a => a.Id == accountId);
        }

        public async Task<Account> AddAccount(Account account)
        {
            var result = _context.Accounts.Add(account);
            await _context.SaveChangesAsync();
            return result.Entity;
        }
    }
}
