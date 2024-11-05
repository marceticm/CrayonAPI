using CrayonAPI.Controllers;
using CrayonAPI.DTOs;
using CrayonAPI.Entities;
using CrayonAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CrayonAPI.UnitTests.Controllers
{
    public class AccountsControllerTests
    {
        private readonly Mock<IAccountService> _mockAccountService;
        private readonly AccountsController _accountsController;

        public AccountsControllerTests()
        {
            _mockAccountService = new Mock<IAccountService>();
            _accountsController = new AccountsController(_mockAccountService.Object);
        }

        [Fact]
        public async Task GetAccounts_WhenAccountsExist_ReturnsOkWithAccounts()
        {
            // Arrange
            var customerId = 1;
            var customer = new Customer { Id = customerId, Name = "Test Customer" };
            var mockService = new Mock<IAccountService>();
            var accounts = new List<AccountResponseDto>
            {
                new AccountResponseDto { Id = 1, CustomerId = customerId, AccountName = "Account 1", CustomerName = "Customer 1" },
                new AccountResponseDto { Id = 2, CustomerId = customerId, AccountName = "Account 2", CustomerName = "Customer 1" }
            };

            mockService.Setup(service => service.GetAccounts(customerId))
                .ReturnsAsync(accounts);

            var controller = new AccountsController(mockService.Object);

            // Act
            var result = await controller.GetAccounts(customerId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedAccounts = Assert.IsType<List<AccountResponseDto>>(okResult.Value);
            Assert.Equal(accounts.Count, returnedAccounts.Count);
        }

        [Fact]
        public async Task GetAccountsByCustomerId_WhenNoAccountsExist_ReturnsNotFound()
        {
            // Arrange
            var customerId = 1;
            var mockService = new Mock<IAccountService>();

            mockService.Setup(service => service.GetAccounts(customerId))
                .ReturnsAsync(new List<AccountResponseDto>());

            var controller = new AccountsController(mockService.Object);

            // Act
            var result = await controller.GetAccounts(customerId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public async Task CreateAccount_WithValidData_ReturnsCreatedResult()
        {
            // Arrange
            var accountDto = new AccountCreateDto { CustomerId = 1, AccountName = "New Account" };
            var createdAccount = new AccountResponseDto { Id = 1, CustomerId = accountDto.CustomerId, AccountName = accountDto.AccountName, CustomerName = "Test Customer" };

            _mockAccountService.Setup(service => service.CreateAccount(accountDto)).ReturnsAsync(createdAccount);

            // Act
            var result = await _accountsController.CreateAccount(accountDto);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnedAccount = Assert.IsType<AccountResponseDto>(createdResult.Value);
            Assert.Equal(createdAccount.Id, returnedAccount.Id);
            Assert.Equal(createdAccount.AccountName, returnedAccount.AccountName);
            _mockAccountService.Verify(service => service.CreateAccount(accountDto), Times.Once);
        }

        [Fact]
        public async Task CreateAccount_WithNullDto_ReturnsBadRequest()
        {
            // Act
            var result = await _accountsController.CreateAccount(null!);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Account data is invalid.", badRequestResult.Value);
            _mockAccountService.Verify(service => service.CreateAccount(It.IsAny<AccountCreateDto>()), Times.Never);
        }

        [Fact]
        public async Task CreateAccount_WithInvalidCustomer_ThrowsArgumentException_ReturnsBadRequest()
        {
            // Arrange
            var accountDto = new AccountCreateDto { CustomerId = 1, AccountName = "New Account" };

            _mockAccountService.Setup(service => service.CreateAccount(accountDto)).ThrowsAsync(new ArgumentException("Invalid CustomerId"));

            // Act
            var result = await _accountsController.CreateAccount(accountDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Invalid CustomerId", badRequestResult.Value);
            _mockAccountService.Verify(service => service.CreateAccount(accountDto), Times.Once);
        }
    }
}
