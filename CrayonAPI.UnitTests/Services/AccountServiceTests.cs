using CrayonAPI.DTOs;
using CrayonAPI.Entities;
using CrayonAPI.Interfaces;
using CrayonAPI.Services;
using Moq;

namespace CrayonAPI.UnitTests.Services
{
    public class AccountServiceTests
    {
        private readonly Mock<IAccountRepository> _mockAccountRepository;
        private readonly Mock<ICustomerRepository> _mockCustomerRepository;
        private readonly AccountService _accountService;

        public AccountServiceTests()
        {
            _mockAccountRepository = new Mock<IAccountRepository>();
            _mockCustomerRepository = new Mock<ICustomerRepository>();
            _accountService = new AccountService(_mockAccountRepository.Object, _mockCustomerRepository.Object);
        }

        [Fact]
        public async Task GetAccount_WhenCalled_ReturnsCorrectAccounts()
        {
            // Arrange
            var customerId = 1;
            var customer = new Customer { Id = customerId, Name = "Test Customer" };

            var mockAccountRepository = new Mock<IAccountRepository>();
            var mockCustomerRepository = new Mock<ICustomerRepository>();

            var expectedAccounts = new List<Account>
            {
                new Account { Id = 1, CustomerId = customerId, AccountName = "Account 1", Customer = customer },
                new Account { Id = 2, CustomerId = customerId, AccountName = "Account 2", Customer = customer }
            };

            var expectedDtos = expectedAccounts.Select(account => new AccountResponseDto
            {
                Id = account.Id,
                CustomerId = account.CustomerId,
                AccountName = account.AccountName,
                CustomerName = account.Customer.Name
            }).ToList();

            mockAccountRepository.Setup(repo => repo.GetAccounts(customerId))
                .ReturnsAsync(expectedAccounts);

            var accountService = new AccountService(mockAccountRepository.Object, mockCustomerRepository.Object);

            // Act
            var result = await accountService.GetAccounts(customerId);

            // Assert
            Assert.Equal(expectedDtos.Count, result.Count());

            foreach (var dto in expectedDtos)
            {
                var actualDto = result.FirstOrDefault(a => a.Id == dto.Id);
                Assert.NotNull(actualDto);
                Assert.Equal(dto.Id, actualDto.Id);
                Assert.Equal(dto.CustomerId, actualDto.CustomerId);
                Assert.Equal(dto.AccountName, actualDto.AccountName);
                Assert.Equal(dto.CustomerName, actualDto.CustomerName);
            }
        }

        [Fact]
        public async Task CreateAccount_WhenCustomerIsValid_CreatesAndReturnsAccountResponseDto()
        {
            // Arrange
            int customerId = 1;
            var accountDto = new AccountCreateDto { CustomerId = customerId, AccountName = "New Account" };
            var customer = new Customer { Id = customerId, Name = "Test Customer" };
            var account = new Account { Id = 1, CustomerId = customerId, AccountName = accountDto.AccountName, Customer = customer };

            _mockCustomerRepository.Setup(repo => repo.GetCustomer(customerId)).ReturnsAsync(customer);
            _mockAccountRepository.Setup(repo => repo.AddAccount(It.IsAny<Account>())).ReturnsAsync(account);

            // Act
            var result = await _accountService.CreateAccount(accountDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(account.Id, result.Id);
            Assert.Equal(customerId, result.CustomerId);
            Assert.Equal(accountDto.AccountName, result.AccountName);
            Assert.Equal(customer.Name, result.CustomerName);
            _mockCustomerRepository.Verify(repo => repo.GetCustomer(customerId), Times.Once);
            _mockAccountRepository.Verify(repo => repo.AddAccount(It.IsAny<Account>()), Times.Once);
        }

        [Fact]
        public async Task CreateAccount_WhenCustomerIsInvalid_ThrowsArgumentException()
        {
            // Arrange
            int customerId = 1;
            var accountDto = new AccountCreateDto { CustomerId = customerId, AccountName = "New Account" };

            _mockCustomerRepository.Setup(repo => repo.GetCustomer(customerId)).ReturnsAsync((Customer?)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _accountService.CreateAccount(accountDto));
            Assert.Equal("Invalid CustomerId", exception.Message);
            _mockCustomerRepository.Verify(repo => repo.GetCustomer(customerId), Times.Once);
            _mockAccountRepository.Verify(repo => repo.AddAccount(It.IsAny<Account>()), Times.Never);
        }
    }
}
