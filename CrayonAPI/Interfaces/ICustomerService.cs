using CrayonAPI.DTOs;
using CrayonAPI.Entities;

namespace CrayonAPI.Interfaces
{
    public interface ICustomerService
    {
        Task<Customer> CreateCustomer(CustomerCreateDto customerDto);
    }
}
