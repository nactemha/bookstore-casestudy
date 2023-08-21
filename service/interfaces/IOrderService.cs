using ecommerce.models;

namespace ecommerce.service
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderEntity>> GetAllOrdersAsync();
         Task<IEnumerable<OrderEntity>> GetCustomerOrdersAsync(int customerId);
        Task<OrderEntity> GetOrderByIdAsync(int id);
        Task PlaceOrderAsync(OrderEntity order);
        Task UpdateOrderAsync(OrderEntity order);
        Task DeleteOrderAsync(int orderId);
        Task<IEnumerable<MonthlyStats>> GetMonthlyStatisticsAsync(int customerId);
    }
}