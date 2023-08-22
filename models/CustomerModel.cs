using System.ComponentModel.DataAnnotations;

namespace ecommerce.models
{
    public class CustomerModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set;}
        public string Email { get; set; }
        [MaxLength(255)]
        public string Address { get; set; }
        [MaxLength(15)]
        public string PhoneNumber { get; set; }

        // Navigation Property
        public ICollection<OrderModel> Orders { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt
        {
            get; set;


        }
    }
}