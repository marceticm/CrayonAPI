using CrayonAPI.DTOs;
using CrayonAPI.Entities;
using CrayonAPI.Enums;
using CrayonAPI.Interfaces;
using CrayonAPI.Services;
using Moq;

namespace CrayonAPI.UnitTests.Services
{
    public class SubscriptionServiceTests
    {
        private readonly Mock<ISubscriptionRepository> _mockSubscriptionRepository;
        private readonly Mock<IAccountRepository> _mockAccountRepository;
        private readonly Mock<ICCPService> _mockCcpService;
        private readonly SubscriptionService _subscriptionService;

        public SubscriptionServiceTests()
        {
            _mockSubscriptionRepository = new Mock<ISubscriptionRepository>();
            _mockAccountRepository = new Mock<IAccountRepository>();
            _mockCcpService = new Mock<ICCPService>();
            _subscriptionService = new SubscriptionService(
                _mockSubscriptionRepository.Object,
                _mockAccountRepository.Object,
                _mockCcpService.Object);
        }

        [Fact]
        public async Task CreateSubscription_WithValidData_CreatesSubscriptionAndReturnsResponseDto()
        {
            // Arrange
            int customerId = 1;
            var subscriptionDto = new SubscriptionCreateDto
            {
                AccountId = 1,
                ServiceCode = 1,
                Quantity = 5,
                ValidTo = DateTime.UtcNow.AddMonths(1)
            };

            var customer = new Customer { Id = customerId, Name = "Test Customer" };
            var account = new Account { Id = 1, CustomerId = customerId, AccountName = "Account 1", Customer = customer };
            var service = new Service { Id = 1, ServiceName = "Service 1", Description = "Description" };
            var subscription = new Subscription
            {
                Id = 1,
                AccountId = subscriptionDto.AccountId,
                ServiceCode = subscriptionDto.ServiceCode,
                Quantity = subscriptionDto.Quantity,
                State = SubscriptionState.Active,
                ValidTo = subscriptionDto.ValidTo,
                Account = account,
            };

            _mockAccountRepository.Setup(repo => repo.GetAccount(subscriptionDto.AccountId)).ReturnsAsync(account);
            _mockCcpService.Setup(repo => repo.GetCCPService(subscriptionDto.ServiceCode)).ReturnsAsync(service);
            _mockSubscriptionRepository.Setup(repo => repo.AddSubscription(It.IsAny<Subscription>())).ReturnsAsync(subscription);

            // Act
            var result = await _subscriptionService.CreateSubscription(customerId, subscriptionDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(subscription.Id, result.Id);
            Assert.Equal(subscription.AccountId, result.AccountId);
            Assert.Equal(subscription.ServiceCode, result.ServiceCode);
            Assert.Equal(subscriptionDto.Quantity, result.Quantity);
            _mockAccountRepository.Verify(repo => repo.GetAccount(subscriptionDto.AccountId), Times.Once);
            _mockCcpService.Verify(repo => repo.GetCCPService(subscriptionDto.ServiceCode), Times.Once);
            _mockSubscriptionRepository.Verify(repo => repo.AddSubscription(It.IsAny<Subscription>()), Times.Once);
        }

        [Fact]
        public async Task CreateSubscription_WithInvalidCustomerId_ThrowsArgumentException()
        {
            // Arrange
            int customerId = 1;
            var subscriptionDto = new SubscriptionCreateDto
            {
                AccountId = 1,
                ServiceCode = 1,
                Quantity = 5,
                ValidTo = DateTime.UtcNow.AddMonths(1)
            };

            _mockAccountRepository.Setup(repo => repo.GetAccount(subscriptionDto.AccountId)).ReturnsAsync((Account?)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
                _subscriptionService.CreateSubscription(customerId, subscriptionDto));
            Assert.Equal("Invalid AccountId or CustomerId", exception.Message);
            _mockAccountRepository.Verify(repo => repo.GetAccount(subscriptionDto.AccountId), Times.Once);
            _mockSubscriptionRepository.Verify(repo => repo.AddSubscription(It.IsAny<Subscription>()), Times.Never);
        }

        [Fact]
        public async Task GetSubscriptionsByAccountId_WithInvalidAccount_ThrowsArgumentException()
        {
            // Arrange
            int accountId = 1;
            int customerId = 1;

            _mockAccountRepository.Setup(repo => repo.GetAccount(accountId)).ReturnsAsync((Account?)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
                _subscriptionService.GetSubscriptionsByAccountId(accountId, customerId));
            Assert.Equal("Invalid AccountId or CustomerId", exception.Message);
            _mockAccountRepository.Verify(repo => repo.GetAccount(accountId), Times.Once);
            _mockSubscriptionRepository.Verify(repo => repo.GetSubscriptionsByAccountId(accountId), Times.Never);
        }

        [Fact]
        public async Task UpdateSubscriptionQuantity_WithValidData_UpdatesQuantityAndReturnsResponseDto()
        {
            // Arrange
            int subscriptionId = 1;
            int customerId = 1;
            int newQuantity = 10;

            var customer = new Customer { Id = customerId, Name = "Test Customer" };
            var account = new Account { Id = 1, CustomerId = customerId, AccountName = "Account 1", Customer = customer };
            var subscription = new Subscription
            {
                Id = subscriptionId,
                AccountId = account.Id,
                ServiceCode = 1,
                Quantity = newQuantity,
                State = SubscriptionState.Active,
                ValidTo = DateTime.UtcNow.AddMonths(1),
                Account = account
            };

            _mockSubscriptionRepository.Setup(repo => repo.GetSubscription(subscriptionId)).ReturnsAsync(subscription);
            _mockSubscriptionRepository.Setup(repo => repo.UpdateSubscriptionQuantity(subscriptionId, newQuantity))
                .ReturnsAsync(subscription);

            // Act
            var result = await _subscriptionService.UpdateSubscriptionQuantity(subscriptionId, customerId, newQuantity);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(newQuantity, result.Quantity);
            _mockSubscriptionRepository.Verify(repo => repo.GetSubscription(subscriptionId), Times.Once);
            _mockSubscriptionRepository.Verify(repo => repo.UpdateSubscriptionQuantity(subscriptionId, newQuantity), Times.Once);
        }

        [Fact]
        public async Task ExtendSubscription_WithValidData_ExtendsValidToDateAndReturnsResponseDto()
        {
            // Arrange
            int subscriptionId = 1;
            int customerId = 1;
            var newValidToDate = DateTime.UtcNow.AddMonths(2);

            var customer = new Customer { Id = customerId, Name = "Test Customer" };
            var account = new Account { Id = 1, CustomerId = customerId, AccountName = "Account 1", Customer = customer };
            var subscription = new Subscription
            {
                Id = subscriptionId,
                AccountId = account.Id,
                ServiceCode = 1,
                Quantity = 5,
                State = SubscriptionState.Active,
                ValidTo = newValidToDate,
                Account = account,
            };

            _mockSubscriptionRepository.Setup(repo => repo.GetSubscription(subscriptionId)).ReturnsAsync(subscription);
            _mockSubscriptionRepository.Setup(repo => repo.ExtendSubscription(subscriptionId, newValidToDate))
                .ReturnsAsync(subscription);

            // Act
            var result = await _subscriptionService.ExtendSubscription(subscriptionId, customerId, newValidToDate);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(newValidToDate, result.ValidTo);
            _mockSubscriptionRepository.Verify(repo => repo.GetSubscription(subscriptionId), Times.Once);
            _mockSubscriptionRepository.Verify(repo => repo.ExtendSubscription(subscriptionId, newValidToDate), Times.Once);
        }

        [Fact]
        public async Task ExtendSubscription_WithInvalidDate_ThrowsArgumentException()
        {
            // Arrange
            int subscriptionId = 1;
            int customerId = 1;
            var invalidValidToDate = DateTime.UtcNow.AddDays(-1); // Past date

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
                _subscriptionService.ExtendSubscription(subscriptionId, customerId, invalidValidToDate));
            Assert.Equal("The new expiration date must be in the future.", exception.Message);
            _mockSubscriptionRepository.Verify(repo => repo.ExtendSubscription(subscriptionId, invalidValidToDate), Times.Never);
        }

        [Fact]
        public async Task CancelSubscription_WithValidData_CancelsSubscriptionAndReturnsResponseDto()
        {
            // Arrange
            int subscriptionId = 1;
            int customerId = 1;

            var customer = new Customer { Id = customerId, Name = "Test Customer" };
            var account = new Account { Id = 1, CustomerId = customerId, AccountName = "Account 1", Customer = customer };
            var subscription = new Subscription
            {
                Id = subscriptionId,
                AccountId = account.Id,
                ServiceCode = 1,
                Quantity = 5,
                State = SubscriptionState.Active,
                ValidTo = DateTime.UtcNow.AddMonths(1),
                Account = account
            };

            var canceledSubscription = subscription;
            canceledSubscription.State = SubscriptionState.Inactive;
            canceledSubscription.ValidTo = DateTime.UtcNow;

            _mockSubscriptionRepository.Setup(repo => repo.GetSubscription(subscriptionId)).ReturnsAsync(subscription);
            _mockSubscriptionRepository.Setup(repo => repo.CancelSubscription(subscriptionId)).ReturnsAsync(canceledSubscription);

            // Act
            var result = await _subscriptionService.CancelSubscription(subscriptionId, customerId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(SubscriptionState.Inactive.ToString(), result.State);
            Assert.True(result.ValidTo <= DateTime.UtcNow);
            _mockSubscriptionRepository.Verify(repo => repo.GetSubscription(subscriptionId), Times.Once);
            _mockSubscriptionRepository.Verify(repo => repo.CancelSubscription(subscriptionId), Times.Once);
        }
    }
}
