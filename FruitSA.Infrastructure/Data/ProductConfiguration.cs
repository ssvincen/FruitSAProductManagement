using FruitSA.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FruitSA.Infrastructure.Data
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(p => p.ProductId);
            builder.HasIndex(p => p.ProductCode).IsUnique();
            builder.HasOne(p => p.Category)
                   .WithMany()
                   .HasForeignKey(p => p.CategoryId)
                   .OnDelete(DeleteBehavior.Restrict);
            builder.Property(p => p.Price).HasPrecision(18, 2);
            builder.Property(p => p.RowVersion).IsConcurrencyToken();

        }
    }
}
