namespace ecommerce.models
{
    public class OrderItemModel
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public decimal SubTotal { get; set; }

        // Foreign Key for Book
        public int BookId { get; set; }
        public BookModel Book { get; set; }

        // Foreign Key for Order
        public int OrderId { get; set; }
        public OrderModel Order { get; set; }
    }
}