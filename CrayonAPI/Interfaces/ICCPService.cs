using CrayonAPI.Entities;

namespace CrayonAPI.Interfaces
{
    public interface ICCPService
    {
        Task<IEnumerable<Service>> GetAvailableServices();
        Task<Service?> GetCCPService(int serviceId);
    }
}
