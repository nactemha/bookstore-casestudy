using ecommerce.data;
using ecommerce.models;

namespace ecommerce.service
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<IEnumerable<CustomerEntity>> GetAllCustomersAsync()
        {
            return await _customerRepository.GetAllAsync();
        }

        public async Task<CustomerEntity> GetCustomerByIdAsync(int id)
        {
            return await _customerRepository.GetByIdAsync(id);
        }
        public async Task<CustomerEntity> GetCustomerByEmailAsync(string email)
        {
            return await _customerRepository.GetCustomerByEmailAsync(email);
        }

        public async Task AddCustomerAsync(CustomerEntity customer)
        {
            if (customer == null)
            {
                throw new ArgumentNullException(nameof(customer));
            }


            await _customerRepository.AddAsync(customer);
            await _customerRepository.SaveChangesAsync();
        }

        public async Task UpdateCustomerAsync(CustomerEntity customer)
        {
            if (customer == null)
            {
                throw new ArgumentNullException(nameof(customer));
            }

            var existingCustomer = await _customerRepository.GetCustomerByEmailAsync(customer.Email);
            if (existingCustomer != null && existingCustomer.Id != customer.Id)
            {
                throw new InvalidOperationException("Another customer with this email already exists.");
            }

            _customerRepository.Update(customer);
            await _customerRepository.SaveChangesAsync();
        }

        public async Task DeleteCustomerAsync(int CustomerId)
        {
            var customer = await _customerRepository.GetByIdAsync(CustomerId);
            if (customer == null)
            {
                throw new InvalidOperationException("Customer not found");
            }

            _customerRepository.Delete(customer.Id);
            await _customerRepository.SaveChangesAsync();
        }
    }


}