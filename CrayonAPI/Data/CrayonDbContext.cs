using CrayonAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace CrayonAPI.Data
{
    public class CrayonDbContext : DbContext
    {
        public CrayonDbContext(DbContextOptions<CrayonDbContext> options) : base(options) { }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
    }
}
