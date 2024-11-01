using CrayonAPI.Entities;

namespace CrayonAPI.Interfaces
{
    public interface ICCPService
    {
        Task<IEnumerable<Service>> GetAvailableServices();
    }
}
