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

        public async Task<IEnumerable<OrderModel>> GetAllOrdersAsync()
        {
            var result = await _orderRepository.GetAllAsync();
            return result.Select(o => new OrderModel
            {
                Id = o.Id,
                CustomerId = o.CustomerId,
                OrderDate = o.OrderDate,
                OrderItems = o.OrderItems.Select(oi => new OrderItemModel
                {
                    BookId = oi.BookId,
                    Quantity = oi.Quantity,
                    SubTotal = oi.SubTotal,
                })
            });
        }
        public async Task<IEnumerable<OrderModel>> GetCustomerOrdersAsync(int customerId)
        {
            var result = await _orderRepository.GetCustomerOrdersAsync(customerId);
            return result.Select(o => new OrderModel
            {
                Id = o.Id,
                CustomerId = o.CustomerId,
                OrderDate = o.OrderDate,
                OrderItems = o.OrderItems.Select(oi => new OrderItemModel
                {
                    BookId = oi.BookId,
                    Quantity = oi.Quantity,
                    SubTotal = oi.SubTotal,
                })
            });
        }

        public async Task<OrderModel> GetOrderByIdAsync(int id)
        {
            var result = await _orderRepository.GetByIdAsync(id);
            return new OrderModel
            {
                Id = result.Id,
                CustomerId = result.CustomerId,
                OrderDate = result.OrderDate,
                OrderItems = result.OrderItems.Select(oi => new OrderItemModel
                {
                    BookId = oi.BookId,
                    Quantity = oi.Quantity,
                    SubTotal = oi.SubTotal,
                })
            };
        }

        public async Task<OrderModel> PlaceOrderAsync(PlaceOrderModel order)
        {
            if (order == null)
            {
                throw new ArgumentNullException(nameof(order));
            }
            OrderEntity orderEntity = null;
            var book = await _bookRepository.GetByIdAsync(order.BookId);

            if (order.Id != null)
            {
                orderEntity = await _orderRepository.GetByIdAsync(order.Id.Value);
                var orderItem = new OrderItemEntity
                {
                    BookId = order.BookId,
                    Quantity = order.Quantity,
                };
                orderItem.SubTotal = order.Quantity * book.Price;
                orderEntity.OrderItems.Add(orderItem);

            }
            else
            {
                orderEntity = new OrderEntity
                {
                    CustomerId = order.CustomerId,
                    OrderDate = DateTime.UtcNow,
                    OrderItems = new List<OrderItemEntity>{
                        new OrderItemEntity{
                            BookId = order.BookId,
                            Quantity = order.Quantity,
                            SubTotal = order.Quantity * book.Price,
                        }
                    }
                };
            }

            await _orderRepository.AddAsync(orderEntity);
            await _orderRepository.SaveChangesAsync();
            return new OrderModel
            {
                Id = orderEntity.Id,
                CustomerId = orderEntity.CustomerId,
                OrderDate = orderEntity.OrderDate,
                OrderItems = orderEntity.OrderItems.Select(oi => new OrderItemModel
                {
                    BookId = oi.BookId,
                    Quantity = oi.Quantity,
                    SubTotal = oi.SubTotal,
                })
            };
        }


        public async Task UpdateOrderAsync(OrderModel order)
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
            var requestOrder = new OrderEntity
            {
                Id = order.Id,
                CustomerId = order.CustomerId,
                OrderDate = order.OrderDate,
                OrderItems = order.OrderItems.Select(oi => new OrderItemEntity
                {
                    BookId = oi.BookId,
                    Quantity = oi.Quantity,
                    SubTotal = oi.SubTotal,
                }).ToList()
            };

            _orderRepository.Update(requestOrder);
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