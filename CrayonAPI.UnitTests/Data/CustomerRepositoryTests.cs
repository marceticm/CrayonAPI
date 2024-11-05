using CrayonAPI.Data;
using CrayonAPI.Entities;

namespace CrayonAPI.UnitTests.Data
{
    public class CustomerRepositoryTests : RepositoryTestBase
    {
        [Fact]
        public async Task GetCustomer_WhenCalled_ReturnsCorrectCustomer()
        {
            // Arrange
            using var context = await GetDbContext();
            var repository = new CustomerRepository(context);
            var customerId = 1;

            var customer = new Customer { Id = customerId, Name = "Test Customer" };
            context.Customers.Add(customer);
            await context.SaveChangesAsync();

            // Act
            var retrievedCustomer = await repository.GetCustomer(customerId);

            // Assert
            Assert.NotNull(retrievedCustomer);
            Assert.Equal(customerId, retrievedCustomer.Id);
            Assert.Equal("Test Customer", retrievedCustomer.Name);
        }

        [Fact]
        public async Task AddCustomer_WhenCalled_AddsCustomerSuccessfully()
        {
            // Arrange
            using var context = await GetDbContext();
            var repository = new CustomerRepository(context);

            var customer = new Customer { Id = 1, Name = "New Customer" };

            // Act
            var addedCustomer = await repository.AddCustomer(customer);

            // Assert
            Assert.NotNull(addedCustomer);
            Assert.Equal(customer.Id, addedCustomer.Id);
            Assert.Equal("New Customer", addedCustomer.Name);
        }
    }
}
