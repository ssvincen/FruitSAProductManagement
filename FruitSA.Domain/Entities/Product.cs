using System.ComponentModel.DataAnnotations;

namespace FruitSA.Domain.Entities
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required]
        [StringLength(10)]
        [RegularExpression(@"^\d{4}(0[1-9]|1[0-2])-\d{3}$", ErrorMessage = "ProductCode must be in format yyyyMM-### (e.g., 202105-023).")]
        public string ProductCode { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(1000)]
        public string? Description { get; set; }

        [Required]
        public int CategoryId { get; set; }

        // Navigation property
        public Category Category { get; set; }

        [Required]
        [Range(0.01, 999999.99)]
        public decimal Price { get; set; }

        public string? ImagePath { get; set; }

        [Required]
        [StringLength(255)]
        public string CreatedBy { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [Timestamp]
        public byte[] RowVersion { get; set; } = new byte[8];
    }
}
