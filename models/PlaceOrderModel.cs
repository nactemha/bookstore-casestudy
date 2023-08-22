using ecommerce.models;

namespace ecommerce.models
{


    public class PlaceOrderModel
    {
        public int? Id { get; set; }
        public int CustomerId { get; set; }
        public int BookId { get; set; }
        public int Quantity
        {
            get; set;


        }
    }
}
