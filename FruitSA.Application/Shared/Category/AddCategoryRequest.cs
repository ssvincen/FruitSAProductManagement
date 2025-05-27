using System.ComponentModel.DataAnnotations;

namespace FruitSA.Application.Shared.Category
{
    public class AddCategoryRequest
    {
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }
        public string Description { get; set; }

        [Display(Name = "Category Code")]
        [Required(ErrorMessage = "Category Code is required.")]
        [StringLength(6)]
        [RegularExpression(@"^[A-Z]{3}\d{3}$", ErrorMessage = "Category Code must be 3 letters followed by 3 numbers (e.g., ABC123).")]
        public string CategoryCode { get; set; }
    }
}
