using CrayonAPI.Entities;
using CrayonAPI.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CrayonAPI.Data
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly CrayonDbContext _context;

        public CustomerRepository(CrayonDbContext context)
        {
            _context = context;
        }

        public async Task<Customer?> GetCustomer(int customerId)
        {
            return await _context.Customers
                .FirstOrDefaultAsync(c => c.Id == customerId);
        }

        public async Task<Customer> AddCustomer(Customer customer)
        {
            var result = await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();
            return result.Entity;
        }
    }
}
