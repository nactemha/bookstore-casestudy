using System.ComponentModel.DataAnnotations;

namespace ecommerce.models
{
    public class CustomerEntity
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string HashedPassword { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        [MaxLength(255)]
        public string Address { get; set; }
        [MaxLength(15)]
        public string PhoneNumber { get; set; }

        // Navigation Property
        public ICollection<OrderEntity> Orders { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt
        {
            get; set;


        }
    }
}