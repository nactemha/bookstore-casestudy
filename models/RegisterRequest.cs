using System.ComponentModel.DataAnnotations;


namespace ecommerce.models
{
    public class RegisterRequest
   {
      [Required]
      [EmailAddress]
      public string Email { get; set; }
      [Required]
      [MinLength(8)]
      [MaxLength(255)]
      public string Password { get; set; }
      [Required]
      public string Name { get; set; }

      [MaxLength(255)]
      public string Address { get; set; }
      [MaxLength(15)]
      [Phone]
      public string PhoneNumber { get; set; }



   }


}