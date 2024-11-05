using CrayonAPI.Controllers;
using CrayonAPI.DTOs;
using CrayonAPI.Entities;
using CrayonAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CrayonAPI.UnitTests.Controllers
{
    public class CustomersControllerTests
    {
        private readonly Mock<ICustomerService> _mockCustomerService;
        private readonly CustomersController _customersController;

        public CustomersControllerTests()
        {
            _mockCustomerService = new Mock<ICustomerService>();
            _customersController = new CustomersController(_mockCustomerService.Object);
        }

        [Fact]
        public async Task CreateCustomer_WithValidData_ReturnsCreatedResult()
        {
            // Arrange
            var customerDto = new CustomerCreateDto { Name = "New Customer" };
            var createdCustomer = new Customer { Id = 1, Name = customerDto.Name };

            _mockCustomerService.Setup(service => service.CreateCustomer(customerDto)).ReturnsAsync(createdCustomer);

            // Act
            var result = await _customersController.CreateCustomer(customerDto);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnedCustomer = Assert.IsType<Customer>(createdResult.Value);
            Assert.Equal(createdCustomer.Id, returnedCustomer.Id);
            Assert.Equal(createdCustomer.Name, returnedCustomer.Name);
            _mockCustomerService.Verify(service => service.CreateCustomer(customerDto), Times.Once);
        }

        [Fact]
        public async Task CreateCustomer_WithNullDto_ReturnsBadRequest()
        {
            // Act
            var result = await _customersController.CreateCustomer(null!);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Customer data is invalid.", badRequestResult.Value);
            _mockCustomerService.Verify(service => service.CreateCustomer(It.IsAny<CustomerCreateDto>()), Times.Never);
        }
    }
}
