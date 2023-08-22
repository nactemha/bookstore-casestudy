using ecommerce.data;
using ecommerce.extension;
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

        public async Task<IEnumerable<CustomerModel>> GetAllCustomersAsync()
        {

            var result= await _customerRepository.GetAllAsync();
            return result.Select(c => new CustomerModel
            {
                Id = c.Id,
                Email = c.Email,
                Address = c.Address,
                PhoneNumber = c.PhoneNumber,
            });
        }

        public async Task<CustomerModel> GetCustomerByIdAsync(int id)
        {
            var result= await _customerRepository.GetByIdAsync(id);
            return new CustomerModel
            {
                Id = result.Id,
                Email = result.Email,
                Address = result.Address,
                PhoneNumber = result.PhoneNumber,
            };
        }
        public async Task<CustomerModel> GetCustomerByEmailAsync(string email)
        {
            var result= await _customerRepository.GetCustomerByEmailAsync(email);
            return new CustomerModel
            {
                Id = result.Id,
                Email = result.Email,
                Address = result.Address,
                PhoneNumber = result.PhoneNumber,
            };
        }

        public async Task AddCustomerAsync(CustomerModel customer)
        {
            
            var existingCustomer = await _customerRepository.GetCustomerByEmailAsync(customer.Email);
            if (existingCustomer != null)
            {
                throw new InvalidOperationException("Another customer with this email already exists.");
            }

            var requestedCustomer =new CustomerEntity{
                Email = customer.Email,
                Address = customer.Address,
                PhoneNumber = customer.PhoneNumber,
                HashedPassword = customer.Password.CalcMD5(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            await _customerRepository.AddAsync(requestedCustomer);
            await _customerRepository.SaveChangesAsync();
        }

        public async Task UpdateCustomerAsync(CustomerModel customer)
        {
            var requestedCustomer = await _customerRepository.GetByIdAsync(customer.Id);
            if (requestedCustomer == null)
            {
                throw new InvalidOperationException("Customer not found.");
            }
            requestedCustomer.Email = customer.Email;
            requestedCustomer.Address = customer.Address;
            requestedCustomer.PhoneNumber = customer.PhoneNumber;
            _customerRepository.Update(requestedCustomer);
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

        public async Task<bool> ValidateCustomerAsync(string email, string passwordHash)
        {
            return await _customerRepository.ValidateCustomerAsync(email, passwordHash);
        }
    }


}