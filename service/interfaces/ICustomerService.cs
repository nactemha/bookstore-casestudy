//create implementation of customer service
using ecommerce.models;

namespace ecommerce.service
{
    public interface ICustomerService
    {
        Task<IEnumerable<CustomerModel>> GetAllCustomersAsync();
        Task<CustomerModel> GetCustomerByIdAsync(int id);
        Task<CustomerModel> GetCustomerByEmailAsync(string email);
        Task<bool> ValidateCustomerAsync(String email, String passwordHash);
        Task AddCustomerAsync(CustomerModel customer);
        Task UpdateCustomerAsync(CustomerModel customer);
        Task DeleteCustomerAsync(int CustomerId);
    }

}