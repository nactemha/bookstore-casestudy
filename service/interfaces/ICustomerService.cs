//create implementation of customer service
using ecommerce.models;

namespace ecommerce.service
{
    public interface ICustomerService
    {
        Task<IEnumerable<CustomerEntity>> GetAllCustomersAsync();
        Task<CustomerEntity> GetCustomerByIdAsync(int id);
        Task<CustomerEntity> GetCustomerByEmailAsync(string email);
        Task AddCustomerAsync(CustomerEntity customer);
        Task UpdateCustomerAsync(CustomerEntity customer);
        Task DeleteCustomerAsync(int CustomerId);
    }

}