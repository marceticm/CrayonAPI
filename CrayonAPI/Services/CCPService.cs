using CrayonAPI.Entities;
using CrayonAPI.Interfaces;

namespace CrayonAPI.Services
{
    public class CCPService : ICCPService
    {
        private readonly List<Service> _services = new List<Service>
        {
            new Service { Id = 1, ServiceName = "Microsoft Office", Description = "MS Office suite", Price = 10.99m },
            new Service { Id = 2, ServiceName = "Adobe Photoshop", Description = "Photo editing software", Price = 19.99m },
            new Service { Id = 3, ServiceName = "Dropbox", Description = "Cloud storage", Price = 5.99m }
        };

        public async Task<IEnumerable<Service>> GetAvailableServices()
        {
            return await Task.FromResult(_services);
        }

        public async Task<Service?> GetCCPService(int serviceId)
        {
            var service = _services.FirstOrDefault(s => s.Id == serviceId);
            return await Task.FromResult(service);
        }
    }
}
