using CrayonAPI.Controllers;
using CrayonAPI.Entities;
using CrayonAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CrayonAPI.UnitTests.Controllers
{
    public class CCPServicesControllerTests
    {
        private readonly Mock<ICCPService> _mockCcpService;
        private readonly CCPServicesController _ccpServicesController;

        public CCPServicesControllerTests()
        {
            _mockCcpService = new Mock<ICCPService>();
            _ccpServicesController = new CCPServicesController(_mockCcpService.Object);
        }

        [Fact]
        public async Task GetAvailableServices_WhenCalled_ReturnsOkResultWithServices()
        {
            // Arrange
            var services = new List<Service>
            {
                new Service { Id = 1, ServiceName = "Service 1", Description = "Description 1" },
                new Service { Id = 2, ServiceName = "Service 2", Description = "Description 2" }
            };

            _mockCcpService.Setup(service => service.GetAvailableServices()).ReturnsAsync(services);

            // Act
            var result = await _ccpServicesController.GetAvailableServices();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedServices = Assert.IsType<List<Service>>(okResult.Value);
            Assert.Equal(2, returnedServices.Count);
            _mockCcpService.Verify(service => service.GetAvailableServices(), Times.Once);
        }
    }
}
