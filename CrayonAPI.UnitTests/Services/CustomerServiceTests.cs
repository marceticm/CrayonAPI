using CrayonAPI.DTOs;
using CrayonAPI.Entities;
using CrayonAPI.Interfaces;
using CrayonAPI.Services;
using Moq;

namespace CrayonAPI.UnitTests.Services
{
    public class CustomerServiceTests
    {
        private readonly Mock<ICustomerRepository> _mockCustomerRepository;
        private readonly CustomerService _customerService;

        public CustomerServiceTests()
        {
            _mockCustomerRepository = new Mock<ICustomerRepository>();
            _customerService = new CustomerService(_mockCustomerRepository.Object);
        }

        [Fact]
        public async Task CreateCustomer_WhenCalled_AddsAndReturnsCustomer()
        {
            // Arrange
            var customerDto = new CustomerCreateDto { Name = "New Customer" };
            var customer = new Customer { Id = 1, Name = customerDto.Name };

            _mockCustomerRepository.Setup(repo => repo.AddCustomer(It.IsAny<Customer>())).ReturnsAsync(customer);

            // Act
            var result = await _customerService.CreateCustomer(customerDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(customer.Name, result.Name);
            _mockCustomerRepository.Verify(repo => repo.AddCustomer(It.IsAny<Customer>()), Times.Once);
        }
    }
}
