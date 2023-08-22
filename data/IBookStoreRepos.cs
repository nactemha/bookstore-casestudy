using ecommerce.models;
namespace ecommerce.data
{
    public interface ICustomerRepository : IRepository<CustomerEntity>
    {
        Task<CustomerEntity> GetCustomerByEmailAsync(string email);
        Task<bool> ValidateCustomerAsync(String email, String passwordHash);
    }
    public interface IBookRepository : IRepository<BookEntity> { }

    public interface IOrderRepository : IRepository<OrderEntity>
    {
        Task<IEnumerable<MonthlyStats>> GetMonthlyStatisticsForCustomerAsync(int customerId);
        Task<List<OrderEntity>> GetCustomerOrdersAsync(int customerId);

    }
}