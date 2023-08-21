using ecommerce.models;
using Nest;

namespace ecommerce.data.elastic
{
    public class CustomerRepository : Repository<CustomerEntity>, ICustomerRepository
    {
        public CustomerRepository(string connectionString, string indexName) : base(connectionString,indexName)
        {

        }

        public async Task<CustomerEntity> GetCustomerByEmailAsync(string email)
        {
            var response = await _client.SearchAsync<CustomerEntity>(s => s
                .Query(q => q
                    .Match(m => m
                        .Field(f => f.Email)
                        .Query(email)
                    )
                )
            );

            return response.Documents.FirstOrDefault();
        }
    }
    public class BookElasticsearchRepository : Repository<BookEntity>, IBookRepository
    {
        public BookElasticsearchRepository(string connectionString, string indexName) : base(connectionString,indexName)
        {
        }

    }
    public class OrderElasticsearchRepository : Repository<OrderEntity>, IOrderRepository
    {
        public OrderElasticsearchRepository(string connectionString, string indexName) : base(connectionString,indexName)
        {
        }

        public async Task<IEnumerable<MonthlyStats>> GetMonthlyStatisticsForCustomerAsync(int customerId)
        {
            var response = await _client.SearchAsync<OrderEntity>(s => s
                .Query(q => q
                    .Match(m => m
                        .Field(f => f.CustomerId)
                        .Query(customerId.ToString())
                    )
                )
                .Aggregations(a => a
                    .DateHistogram("monthly_stats", dh => dh
                        .Field(f => f.OrderDate)
                        .CalendarInterval(DateInterval.Month)
                        .Aggregations(aa => aa
                            .Sum("total_amount", sa => sa
                                .Field(f => f.TotalAmount)
                            )
                            .Sum("total_books_purchased", sa => sa
                                .Field(f => f.OrderItems.Sum(oi => oi.Quantity))
                            )
                        )
                    )
                )
            );

            var monthlyStats = new List<MonthlyStats>();
            foreach (var bucket in response.Aggregations.DateHistogram("monthly_stats").Buckets)
            {
                monthlyStats.Add(new MonthlyStats
                {
                    Year = bucket.Date.Year,
                    Month = bucket.Date.Month,
                    TotalOrderCount = Convert.ToInt32(bucket.DocCount??0),
                    TotalAmount = Convert.ToDecimal(bucket.Sum("total_amount").Value??0),
                    TotalBooksPurchased = Convert.ToInt32(bucket.Sum("total_books_purchased").Value??0)
                });
            }

            return monthlyStats;
        }
          
        public async Task<List<OrderEntity>> GetCustomerOrdersAsync(int customerId)
        {
            var response = await _client.SearchAsync<OrderEntity>(s => s
                .Query(q => q
                    .Match(m => m
                        .Field(f => f.CustomerId)
                        .Query(customerId.ToString())
                    )
                )
            );

            return response.Documents.ToList();
        }
    }


}