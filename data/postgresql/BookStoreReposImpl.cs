
using ecommerce.models;
using Microsoft.EntityFrameworkCore;

namespace ecommerce.data.postgre
{
    public class CustomerRepository : Repository<CustomerEntity>, ecommerce.data.ICustomerRepository
    {
        public CustomerRepository(AppDbContext context) : base(context)
        {

        }

        public async Task<CustomerEntity> GetCustomerByEmailAsync(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(c => c.Email == email);
        }
    }

    public class BookRepository : Repository<BookEntity>, IBookRepository
    {
        public BookRepository(AppDbContext context) : base(context)
        {
        }
    }

    public class OrderRepository : Repository<OrderEntity>, IOrderRepository
    {
        private DbSet<OrderEntity> _orders;

        public OrderRepository(AppDbContext context) : base(context)
        {
            _orders = context.Set<OrderEntity>();
        }
        public async Task<IEnumerable<MonthlyStats>> GetMonthlyStatisticsForCustomerAsync(int customerId)
        {
            return await _orders
                .Where(order => order.CustomerId == customerId)
                .GroupBy(order => new { Year = order.OrderDate.Year, Month = order.OrderDate.Month })
                .Select(group => new MonthlyStats
                {
                    Year = group.Key.Year,
                    Month = group.Key.Month,
                    TotalOrderCount = group.Count(),
                    TotalAmount = group.Sum(order => order.TotalAmount),
                    TotalBooksPurchased = group.Sum(order => order.OrderItems.Sum(item => item.Quantity))
                })
                .ToListAsync();
        }
        public Task<List<OrderEntity>> GetCustomerOrdersAsync(int customerId)
        {
            return _orders.Where(order => order.CustomerId == customerId).ToListAsync();
        }



    }
}