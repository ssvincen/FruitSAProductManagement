using System.ComponentModel.DataAnnotations;

namespace FruitSA.Application.Shared.Product
{
    public class AddProductRequest
    {
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }
        public string Description { get; set; }

        [Display(Name = "Category")]
        [Required(ErrorMessage = "Category is required.")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
        public decimal Price { get; set; }
        public string ImagePath { get; set; }

    }
}
