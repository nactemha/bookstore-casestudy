using ecommerce.data;
using ecommerce.models;
using ecommerce.service;
using Moq;
using Xunit;

namespace ecommerce.Tests.services
{
    public class OrderServiceTests
    {
        private Mock<IOrderRepository> _mockOrderRepo;
        private Mock<IBookRepository> _mockBookRepo;
        private OrderService _service;

        public OrderServiceTests()
        {
            _mockOrderRepo = new Mock<IOrderRepository>();
            _mockBookRepo = new Mock<IBookRepository>();
            _service = new OrderService(_mockOrderRepo.Object, _mockBookRepo.Object);
        }

        [Fact]
        public async Task GetAllOrdersAsync_ReturnsListOfOrders()
        {
            // Arrange
            var orders = new List<OrderEntity> { new OrderEntity { Id = 1 }, new OrderEntity { Id = 2 } };
            _mockOrderRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(orders);

            // Act
            var result = await _service.GetAllOrdersAsync();

            // Assert
            Assert.Equal(orders, result);
        }

    }
}