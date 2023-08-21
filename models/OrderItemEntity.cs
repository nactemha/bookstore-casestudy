namespace ecommerce.models
{
    public class OrderItemEntity
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public decimal SubTotal { get; set; }

        // Foreign Key for Book
        public int BookId { get; set; }
        public BookEntity Book { get; set; }

        // Foreign Key for Order
        public int OrderId { get; set; }
        public OrderEntity Order { get; set; }
    }
}