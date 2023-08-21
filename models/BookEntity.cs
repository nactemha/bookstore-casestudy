
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ecommerce.models
{
public class BookEntity
{
    public int Id { get; set; }
    [MaxLength(255)]
    public string Title { get; set; }
    public string Author { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
    
    // Navigation Property
    public ICollection<OrderItemEntity> OrderItems { get; set; }
}
}