using CrayonAPI.Data;
using CrayonAPI.Entities;

namespace CrayonAPI.UnitTests.Data
{
    public class AccountRepositoryTests : RepositoryTestBase
    {
        [Fact]
        public async Task GetAccountsByCustomerIdAsync_WhenCalled_ReturnsCorrectAccounts()
        {
            // Arrange
            using var context = await GetDbContext();
            var repository = new AccountRepository(context);
            var customerId = 1;

            var customer = new Customer { Id = customerId, Name = "Test Customer" };
            context.Customers.Add(customer);

            context.Accounts.Add(new Account { Id = 1, CustomerId = customerId, AccountName = "Account 1", Customer = customer });
            context.Accounts.Add(new Account { Id = 2, CustomerId = customerId, AccountName = "Account 2", Customer = customer });
            context.Accounts.Add(new Account { Id = 3, CustomerId = 2, AccountName = "Account 3", Customer = new Customer { Id = 2, Name = "Other Customer" } });
            await context.SaveChangesAsync();

            // Act
            var accounts = await repository.GetAccounts(customerId);

            // Assert
            Assert.Equal(2, accounts.Count());
            Assert.All(accounts, a => Assert.Equal(customerId, a.CustomerId));
        }

        [Fact]
        public async Task GetAccount__WhenCalled_ReturnsCorrectAccountById()
        {
            // Arrange
            using var context = await GetDbContext();
            var repository = new AccountRepository(context);
            var accountId = 1;

            var customer = new Customer { Id = 1, Name = "Test Customer" };
            var account = new Account
            {
                Id = accountId,
                CustomerId = customer.Id,
                AccountName = "Account 1",
                Customer = customer
            };

            context.Customers.Add(customer);
            context.Accounts.Add(account);
            await context.SaveChangesAsync();

            // Act
            var retrievedAccount = await repository.GetAccount(accountId);

            // Assert
            Assert.NotNull(retrievedAccount);
            Assert.Equal(accountId, retrievedAccount.Id);
            Assert.Equal(customer.Id, retrievedAccount.CustomerId);
            Assert.Equal("Account 1", retrievedAccount.AccountName);
            Assert.NotNull(retrievedAccount.Customer);
            Assert.Equal("Test Customer", retrievedAccount.Customer.Name);
        }

        [Fact]
        public async Task AddAccount_WhenCalled_AddsAccountSuccessfully()
        {
            // Arrange
            using var context = await GetDbContext();
            var repository = new AccountRepository(context);

            var customer = new Customer { Id = 1, Name = "Test Customer" };
            context.Customers.Add(customer);
            await context.SaveChangesAsync();

            var account = new Account
            {
                Id = 1,
                CustomerId = customer.Id,
                AccountName = "New Account",
                Customer = customer
            };

            // Act
            var addedAccount = await repository.AddAccount(account);

            // Assert
            Assert.NotNull(addedAccount);
            Assert.Equal(account.Id, addedAccount.Id);
            Assert.Equal(account.CustomerId, addedAccount.CustomerId);
            Assert.Equal(account.AccountName, addedAccount.AccountName);
            Assert.NotNull(addedAccount.Customer);
            Assert.Equal("Test Customer", addedAccount.Customer.Name);
        }
    }
}