using ecommerce.models;

namespace ecommerce.models
{


    public class OrderModel
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderStatus Status { get; set; }
        public decimal TotalAmount { get; set; }

        // Foreign Key for Customer
        public int CustomerId { get; set; }
        public CustomerModel Customer { get; set; }

        // Navigation Property
        public IEnumerable<OrderItemModel> OrderItems { get; set; }
    }
}