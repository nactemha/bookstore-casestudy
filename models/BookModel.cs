
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ecommerce.models
{
public class BookModel
{
    public int Id { get; set; }
    [MaxLength(255)]
    public string Title { get; set; }
    public string Author { get; set; }
    public decimal Price { get; set; }
    
    }
}