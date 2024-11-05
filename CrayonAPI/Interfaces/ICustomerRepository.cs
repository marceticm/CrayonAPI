using CrayonAPI.Entities;

namespace CrayonAPI.Interfaces
{
    public interface ICustomerRepository
    {
        Task<Customer?> GetCustomer(int customerId);
        Task<Customer> AddCustomer(Customer customer);
    }
}
