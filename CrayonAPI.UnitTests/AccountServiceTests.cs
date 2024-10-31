using CrayonAPI.Entities;
using CrayonAPI.Interfaces;
using CrayonAPI.Services;
using Moq;

namespace CrayonAPI.UnitTests
{
    public class AccountServiceTests
    {
        [Fact]
        public async Task GetAccountsByCustomerId_ReturnsCorrectAccounts()
        {
            // Arrange
            var customerId = 1;
            var customer = new Customer { Id = customerId, Name = "Test Customer" };

            var mockRepository = new Mock<IAccountRepository>();

            var expectedAccounts = new List<Account>
            {
                new Account { Id = 1, CustomerId = customerId, AccountName = "Account 1", Customer = customer },
                new Account { Id = 2, CustomerId = customerId, AccountName = "Account 2", Customer = customer }
            };

            mockRepository.Setup(repo => repo.GetAccounts(customerId))
                .ReturnsAsync(expectedAccounts);

            var accountService = new AccountService(mockRepository.Object);

            // Act
            var result = await accountService.GetAccounts(customerId);

            // Assert
            Assert.Equal(expectedAccounts.Count, result.Count());
            Assert.Equal(expectedAccounts, result);
        }
    }
}
