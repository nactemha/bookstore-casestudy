
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ecommerce.models
{
   public class  ResponseData<T>
   {
       public int Status { get; set; }
       public T Data { get; set; }

       
    }


}