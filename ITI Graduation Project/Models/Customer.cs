using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ITI_Graduation_Project.Models
{
    public class Customer
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "You have to provide a valid full name.")]
        [MinLength(6, ErrorMessage = "Full name mustn't be less than 6 characters.")]
        [MaxLength(60, ErrorMessage = "Full name mustn't exceed 60 characters.")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "You have to provide a valid full name.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "You have to provide a valid Password.")]
        public string Password { get; set; }

        // Nullable Values
        public int? Phone { get; set; }
        public string? Address { get; set; }


        [Range(1, int.MaxValue, ErrorMessage = "Select a valid Product.")]

        [ValidateNever]
        [DeleteBehavior(DeleteBehavior.NoAction)]
        public List<Product> Products { get; set; }
    }
}
