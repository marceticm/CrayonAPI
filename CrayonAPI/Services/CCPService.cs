using CrayonAPI.Entities;
using CrayonAPI.Interfaces;

namespace CrayonAPI.Services
{
    public class CCPService : ICCPService
    {
        public async Task<IEnumerable<Service>> GetAvailableServices()
        {
            var services = new List<Service>
            {
                new Service { Id = 1, ServiceName = "Microsoft Office", Description = "MS Office suite", Price = 10.99m },
                new Service { Id = 2, ServiceName = "Adobe Photoshop", Description = "Photo editing software", Price = 19.99m },
                new Service { Id = 3, ServiceName = "Dropbox", Description = "Cloud storage", Price = 5.99m }
            };

            return await Task.FromResult(services);
        }
    }
}
