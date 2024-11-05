using CrayonAPI.Controllers;
using CrayonAPI.DTOs;
using CrayonAPI.Enums;
using CrayonAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CrayonAPI.UnitTests.Controllers
{
    public class SubscriptionsControllerTests
    {
        private readonly Mock<ISubscriptionService> _mockSubscriptionService;
        private readonly SubscriptionsController _subscriptionsController;

        public SubscriptionsControllerTests()
        {
            _mockSubscriptionService = new Mock<ISubscriptionService>();
            _subscriptionsController = new SubscriptionsController(_mockSubscriptionService.Object);
        }

        [Fact]
        public async Task PurchaseSubscription_WithValidData_ReturnsCreatedResult()
        {
            // Arrange
            int customerId = 1;
            var subscriptionDto = new SubscriptionCreateDto { AccountId = 1, ServiceCode = 1, Quantity = 5, ValidTo = DateTime.UtcNow.AddMonths(1) };
            var createdSubscription = new SubscriptionResponseDto
            {
                Id = 1,
                AccountId = subscriptionDto.AccountId,
                ServiceCode = subscriptionDto.ServiceCode,
                Quantity = subscriptionDto.Quantity,
                State = SubscriptionState.Active.ToString(),
                AccountName = "Test Account"
            };

            _mockSubscriptionService.Setup(service => service.CreateSubscription(customerId, subscriptionDto)).ReturnsAsync(createdSubscription);

            // Act
            var result = await _subscriptionsController.PurchaseSubscription(customerId, subscriptionDto);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnedSubscription = Assert.IsType<SubscriptionResponseDto>(createdResult.Value);
            Assert.Equal(createdSubscription.Id, returnedSubscription.Id);
            Assert.Equal(createdSubscription.State, returnedSubscription.State);
            _mockSubscriptionService.Verify(service => service.CreateSubscription(customerId, subscriptionDto), Times.Once);
        }

        [Fact]
        public async Task PurchaseSubscription_WithNullDto_ReturnsBadRequest()
        {
            // Act
            var result = await _subscriptionsController.PurchaseSubscription(1, null!);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Subscription data is invalid.", badRequestResult.Value);
            _mockSubscriptionService.Verify(service => service.CreateSubscription(It.IsAny<int>(), It.IsAny<SubscriptionCreateDto>()), Times.Never);
        }

        [Fact]
        public async Task GetSubscriptionsByAccountId_WithValidData_ReturnsOkResultWithSubscriptions()
        {
            // Arrange
            int customerId = 1;
            int accountId = 1;
            var subscriptions = new List<SubscriptionResponseDto>
            {
                new SubscriptionResponseDto { Id = 1, AccountId = accountId, ServiceCode = 1, Quantity = 5, State = SubscriptionState.Active.ToString(), AccountName = "Test Account" },
                new SubscriptionResponseDto { Id = 2, AccountId = accountId, ServiceCode = 2, Quantity = 10, State = SubscriptionState.Active.ToString(), AccountName = "Test Account" }
            };

            _mockSubscriptionService.Setup(service => service.GetSubscriptionsByAccountId(accountId, customerId)).ReturnsAsync(subscriptions);

            // Act
            var result = await _subscriptionsController.GetSubscriptionsByAccountId(customerId, accountId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedSubscriptions = Assert.IsType<List<SubscriptionResponseDto>>(okResult.Value);
            Assert.Equal(2, returnedSubscriptions.Count);
            _mockSubscriptionService.Verify(service => service.GetSubscriptionsByAccountId(accountId, customerId), Times.Once);
        }

        [Fact]
        public async Task UpdateSubscriptionQuantity_WithValidData_ReturnsOkResult()
        {
            // Arrange
            int customerId = 1;
            int subscriptionId = 1;
            int quantity = 10;
            var updatedSubscription = new SubscriptionResponseDto
            {
                Id = subscriptionId,
                Quantity = quantity,
                State = SubscriptionState.Active.ToString(),
                AccountName = "Test Account"
            };

            _mockSubscriptionService.Setup(service => service.UpdateSubscriptionQuantity(subscriptionId, customerId, quantity)).ReturnsAsync(updatedSubscription);

            // Act
            var result = await _subscriptionsController.UpdateSubscriptionQuantity(customerId, subscriptionId, quantity);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedSubscription = Assert.IsType<SubscriptionResponseDto>(okResult.Value);
            Assert.Equal(quantity, returnedSubscription.Quantity);
            Assert.Equal(updatedSubscription.State, returnedSubscription.State);
            _mockSubscriptionService.Verify(service => service.UpdateSubscriptionQuantity(subscriptionId, customerId, quantity), Times.Once);
        }

        [Fact]
        public async Task CancelSubscription_WithValidData_ReturnsOkResult()
        {
            // Arrange
            int customerId = 1;
            int subscriptionId = 1;
            var canceledSubscription = new SubscriptionResponseDto
            {
                Id = subscriptionId,
                State = SubscriptionState.Inactive.ToString(),
                AccountName = "Test Account"
            };

            _mockSubscriptionService.Setup(service => service.CancelSubscription(subscriptionId, customerId)).ReturnsAsync(canceledSubscription);

            // Act
            var result = await _subscriptionsController.CancelSubscription(customerId, subscriptionId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedSubscription = Assert.IsType<SubscriptionResponseDto>(okResult.Value);
            Assert.Equal(SubscriptionState.Inactive.ToString(), returnedSubscription.State);
            _mockSubscriptionService.Verify(service => service.CancelSubscription(subscriptionId, customerId), Times.Once);
        }

        [Fact]
        public async Task ExtendSubscription_WithValidData_ReturnsOkResult()
        {
            // Arrange
            int customerId = 1;
            int subscriptionId = 1;
            var newValidToDate = DateTime.UtcNow.AddMonths(2);
            var extendedSubscription = new SubscriptionResponseDto
            {
                Id = subscriptionId,
                ValidTo = newValidToDate,
                State = SubscriptionState.Active.ToString(),
                AccountName = "Test Account"
            };

            _mockSubscriptionService.Setup(service => service.ExtendSubscription(subscriptionId, customerId, newValidToDate)).ReturnsAsync(extendedSubscription);

            // Act
            var result = await _subscriptionsController.ExtendSubscription(customerId, subscriptionId, newValidToDate);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedSubscription = Assert.IsType<SubscriptionResponseDto>(okResult.Value);
            Assert.Equal(newValidToDate, returnedSubscription.ValidTo);
            Assert.Equal(extendedSubscription.State, returnedSubscription.State);
            _mockSubscriptionService.Verify(service => service.ExtendSubscription(subscriptionId, customerId, newValidToDate), Times.Once);
        }
    }
}
