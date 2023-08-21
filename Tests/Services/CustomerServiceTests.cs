using ecommerce.data;
using ecommerce.models;
using ecommerce.service;
using Moq;
using Xunit;

namespace ecommerce.Tests.services
{


    public class CustomerServiceTests
    {
        private Mock<ICustomerRepository> _mockRepo;
        private Mock<ISettings> _mockSettings;
        private CustomerService _service;

        public CustomerServiceTests()
        {
            _mockRepo = new Mock<ICustomerRepository>();
            _service = new CustomerService(_mockRepo.Object);
        }

        [Fact]
        public async Task GetAllCustomersAsync_ReturnsListOfCustomers()
        {
            // Arrange
            var customers = new List<CustomerEntity> { new CustomerEntity { Id = 1, Name = "John" }, new CustomerEntity { Id = 2, Name = "Jane" } };
            _mockRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(customers);

            // Act
            var result = await _service.GetAllCustomersAsync();

            // Assert
            Assert.Equal(customers, result);
        }

    }

}