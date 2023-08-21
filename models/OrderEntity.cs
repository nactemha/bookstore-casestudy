using ecommerce.models;

namespace ecommerce.models
{
    public enum OrderStatus
{
    Pending,
    Shipped,
    Delivered,
    Canceled
}

public class OrderEntity
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; }
    public OrderStatus Status { get; set; }
    public decimal TotalAmount { get; set; }

    // Foreign Key for Customer
    public int CustomerId { get; set; }
    public CustomerEntity Customer { get; set; }

    // Navigation Property
    public ICollection<OrderItemEntity> OrderItems { get; set; }
}
}