using ecommerce.models;

namespace ecommerce.service
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderModel>> GetAllOrdersAsync();
         Task<IEnumerable<OrderModel>> GetCustomerOrdersAsync(int customerId);
        Task<OrderModel> GetOrderByIdAsync(int id);
        Task<OrderModel> PlaceOrderAsync(PlaceOrderModel order);
        Task UpdateOrderAsync(OrderModel order);
        Task DeleteOrderAsync(int orderId);
        Task<IEnumerable<MonthlyStats>> GetMonthlyStatisticsAsync(int customerId);
    }
}