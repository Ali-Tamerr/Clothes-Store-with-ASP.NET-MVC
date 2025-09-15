using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace ITI_Graduation_Project.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "You have to provide a valid full name.")]
        [MinLength(10, ErrorMessage = "Full name mustn't be less than 10 characters.")]
        [MaxLength(60, ErrorMessage = "Full name mustn't exceed 60 characters.")]
        public string Name { get; set; }

        [MinLength(10, ErrorMessage = "Description name mustn't be less than 10 characters.")]
        [Required(ErrorMessage = "You have to provide a valid description.")]
        [MaxLength(200, ErrorMessage = "Description mustn't exceed 200 characters.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "You have to provide a valid value for age.")]
        [MaxLength(3, ErrorMessage = "ForAge's value mustn't exceed 3 characters.")]

        public string ForAge { get; set; }

        // Persisted column: stores saved image path / filename
        public string? ImagePath { get; set; }

        // Not mapped: used only to bind the uploaded file in forms
        [NotMapped]
        [ValidateNever]
        [DataType(DataType.Upload)]
        public IFormFile? ImageFile { get; set; }

        [ValidateNever]
        [DeleteBehavior(DeleteBehavior.NoAction)]
        public List<Customer> Customers { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Select a valid category.")]
        public int CategoryId { get; set; }

        [ValidateNever]
        [DeleteBehavior(DeleteBehavior.NoAction)]
        public Category Category { get; set; }

    }
}
