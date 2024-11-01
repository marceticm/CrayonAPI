using CrayonAPI.Data;
using CrayonAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace CrayonAPI.UnitTests.Data
{
    public class AccountRepositoryTests
    {
        [Fact]
        public async Task GetAccountsByCustomerIdAsync_WhenCalled_ReturnsCorrectAccounts()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<CrayonDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var context = new CrayonDbContext(options))
            {
                // Adding customer to in-memory db
                var customer = new Customer { Id = 1, Name = "Test Customer" };
                context.Customers.Add(customer);

                // Creating and adding customer accounts
                var accounts = new List<Account>
                {
                    new Account { Id = 1, CustomerId = 1, AccountName = "Account 1", Customer = customer },
                    new Account { Id = 2, CustomerId = 1, AccountName = "Account 2", Customer = customer },
                    new Account { Id = 3, CustomerId = 2, AccountName = "Account 3", Customer = new Customer { Id = 2, Name = "Another Customer" } }
                };

                context.Accounts.AddRange(accounts);
                await context.SaveChangesAsync();
            }

            using (var context = new CrayonDbContext(options))
            {
                var repository = new AccountRepository(context);

                // Act
                var result = await repository.GetAccounts(1);

                // Assert
                Assert.Equal(2, result.Count()); // Expect 2 accounts for CustomerId = 1
                Assert.Contains(result, a => a.AccountName == "Account 1");
                Assert.Contains(result, a => a.AccountName == "Account 2");
            }
        }
    }
}