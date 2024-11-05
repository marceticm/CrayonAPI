using CrayonAPI.DTOs;
using CrayonAPI.Entities;
using CrayonAPI.Interfaces;

namespace CrayonAPI.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<Customer> CreateCustomer(CustomerCreateDto customerDto)
        {
            var customer = new Customer { Name = customerDto.Name };

            await _customerRepository.AddCustomer(customer);
            return customer;
        }
    }
}
