using System.ComponentModel.DataAnnotations;

namespace FruitSA.Domain.Entities
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }


        [StringLength(255)]
        public string? Description { get; set; }

        [Required]
        [StringLength(6)]
        [RegularExpression(@"^[A-Z]{3}\d{3}$", ErrorMessage = "CategoryCode must be 3 letters followed by 3 numbers (e.g., ABC123).")]
        public string CategoryCode { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;

        [Required]
        [StringLength(255)]
        public string CreatedBy { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [Timestamp]
        public byte[] RowVersion { get; set; } = new byte[8];
    }
}

