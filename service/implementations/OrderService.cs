using ecommerce.data;
using ecommerce.models;

namespace ecommerce.service
{

    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IBookRepository _bookRepository; // We might need this to adjust the book stocks.

        public OrderService(IOrderRepository orderRepository, IBookRepository bookRepository)
        {
            _orderRepository = orderRepository;
            _bookRepository = bookRepository;
        }

        public async Task<IEnumerable<OrderEntity>> GetAllOrdersAsync()
        {
            return await _orderRepository.GetAllAsync();
        }
        public async Task<IEnumerable<OrderEntity>> GetCustomerOrdersAsync(int customerId)
        {
            return await _orderRepository.GetCustomerOrdersAsync(customerId);
        }

        public async Task<OrderEntity> GetOrderByIdAsync(int id)
        {
            return await _orderRepository.GetByIdAsync(id);
        }

        public async Task PlaceOrderAsync(OrderEntity order)
        {
            if (order == null)
            {
                throw new ArgumentNullException(nameof(order));
            }

            var errorMessage = "";
            var books = new List<BookEntity>();
            foreach (var orderItem in order.OrderItems)
            {
                var book = await _bookRepository.GetByIdAsync(orderItem.BookId);
                if (book == null)
                {
                    errorMessage += $"Book with id {orderItem.BookId} does not exist.";
                    continue;

                }

                if (book.Stock < orderItem.Quantity)
                {
                    errorMessage += $"Book with id {orderItem.BookId} is out of stock.";
                    continue;
                }

                // Adjust the book stock
                book.Stock -= orderItem.Quantity;
            }
            if (errorMessage != "")
            {
                throw new InvalidOperationException(errorMessage);
            }
            foreach (var book in books)
            {
                _bookRepository.Update(book);
            }
            await _orderRepository.AddAsync(order);
            await _orderRepository.SaveChangesAsync();
        }


        public async Task UpdateOrderAsync(OrderEntity order)
        {
            if (order == null)
            {
                throw new ArgumentNullException(nameof(order));
            }

            var existingOrder = await _orderRepository.GetByIdAsync(order.Id);
            if (existingOrder == null)
            {
                throw new InvalidOperationException("Order does not exist.");
            }
            var errorMessage = "";
            var books = new List<BookEntity>();
            foreach (var orderItem in order.OrderItems)
            {
                var book = await _bookRepository.GetByIdAsync(orderItem.BookId);
                if (book == null)
                {
                    errorMessage += $"Book with id {orderItem.BookId} does not exist.";
                    continue;
                }

                if (book.Stock < orderItem.Quantity)
                {
                    errorMessage += $"Book with id {orderItem.BookId} is out of stock.";
                    continue;
                }

                // Adjust the book stock
                book.Stock -= orderItem.Quantity;
                books.Add(book);
            }
            if (errorMessage != "")
            {
                throw new InvalidOperationException(errorMessage);
            }
            foreach (var book in books)
            {
                _bookRepository.Update(book);
            }

            _orderRepository.Update(order);
            await _orderRepository.SaveChangesAsync();
        }

        public async Task DeleteOrderAsync(int orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null)
            {
                throw new InvalidOperationException("Order does not exist.");
            }
            var errorMessage = "";
            var books = new List<BookEntity>();
            foreach (var orderItem in order.OrderItems)
            {
                var book = await _bookRepository.GetByIdAsync(orderItem.BookId);
                if (book == null)
                {
                    errorMessage += $"Book with id {orderItem.BookId} does not exist.";
                    continue;
                }

                // Adjust the book stock
                book.Stock += orderItem.Quantity;
                books.Add(book);
            }
            foreach (var book in books)
            {
                _bookRepository.Update(book);
            }
            _orderRepository.Delete(order.Id);
            await _orderRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<MonthlyStats>> GetMonthlyStatisticsAsync(int customerId)
        {
            return await _orderRepository.GetMonthlyStatisticsForCustomerAsync(customerId);
        }


    }
}