using CrayonAPI.Controllers;
using CrayonAPI.Entities;
using CrayonAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CrayonAPI.UnitTests.Controllers
{
    public  class AccountControllerTests
    {
        [Fact]
        public async Task GetAccounts_WhenAccountsExist_ReturnsOkWithAccounts()
        {
            // Arrange
            var customerId = 1;
            var customer = new Customer { Id = customerId, Name = "Test Customer" };
            var mockService = new Mock<IAccountService>();
            var accounts = new List<Account>
            {
                new Account { Id = 1, CustomerId = customerId, AccountName = "Account 1", Customer = customer },
                new Account { Id = 2, CustomerId = customerId, AccountName = "Account 2", Customer = customer }
            };

            mockService.Setup(service => service.GetAccounts(customerId))
                .ReturnsAsync(accounts);

            var controller = new AccountsController(mockService.Object);

            // Act
            var result = await controller.GetAccounts(customerId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedAccounts = Assert.IsType<List<Account>>(okResult.Value);
            Assert.Equal(accounts.Count, returnedAccounts.Count);
        }

        [Fact]
        public async Task GetAccountsByCustomerId_WhenNoAccountsExist_ReturnsNotFound()
        {
            // Arrange
            var customerId = 1;
            var mockService = new Mock<IAccountService>();

            mockService.Setup(service => service.GetAccounts(customerId))
                .ReturnsAsync(new List<Account>());

            var controller = new AccountsController(mockService.Object);

            // Act
            var result = await controller.GetAccounts(customerId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }
    }
}
