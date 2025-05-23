using FruitSA.Domain.Constant;
using FruitSA.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FruitSA.Infrastructure.Data
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>());

            context.Database.EnsureCreated();

            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            string[] roles = [UserRolesConst.Admin, UserRolesConst.Manager, UserRolesConst.General];
            foreach (var role in roles)
            {
                if (!roleManager.RoleExistsAsync(role).Result)
                {
                    roleManager.CreateAsync(new IdentityRole(role)).Wait();
                }
            }

            if (context.Categories.Any() || context.Products.Any())
            {
                return; // Database already seeded
            }

            var categories = new Category[]
            {
                new() {
                    Name = "Fruits",
                    CategoryCode = "FRT123",
                    Description = "A category for all types of fruits, fresh and processed.",
                    IsActive = true,
                    CreatedBy = "System",
                    CreatedDate = DateTime.UtcNow
                },
                new() {
                    Name = "Vegetables",
                    CategoryCode = "VEG456",
                    Description = "A category for all varieties of vegetables, organic and conventional.",
                    IsActive = true,
                    CreatedBy = "System",
                    CreatedDate = DateTime.UtcNow
                }
            };
            context.Categories.AddRange(categories);
            context.SaveChanges();

            // After saving categories, we can set the CategoryId for products
            int fruitsCategoryId = categories[0].CategoryId;
            int vegetablesCategoryId = categories[1].CategoryId;
            var products = new Product[]
            {
                // Fruits (CategoryId will be set after saving categories)
                new() { ProductCode = "202505-001", Name = "Apple", Description = "Fresh red apple", CategoryId = fruitsCategoryId, Price = 7.00m, CreatedBy = "System", CreatedDate = DateTime.UtcNow },
                new() { ProductCode = "202505-002", Name = "Banana", Description = "Ripe yellow banana", CategoryId = fruitsCategoryId, Price = 15.00m, CreatedBy = "System", CreatedDate = DateTime.UtcNow },
                new() { ProductCode = "202505-003", Name = "Orange", Description = "Juicy orange", CategoryId = fruitsCategoryId, Price = 15.00m, CreatedBy = "System", CreatedDate = DateTime.UtcNow },
                new() { ProductCode = "202505-004", Name = "Mango", Description = "Sweet mango", CategoryId = fruitsCategoryId, Price = 20.00m, CreatedBy = "System", CreatedDate = DateTime.UtcNow },
                new() { ProductCode = "202505-005", Name = "Pineapple", Description = "Tropical pineapple", CategoryId = fruitsCategoryId, Price = 25.00m, CreatedBy = "System", CreatedDate = DateTime.UtcNow },
                new() { ProductCode = "202505-006", Name = "Avocado", Description = "Ripe Avocado", CategoryId = fruitsCategoryId, Price = 10.00m, CreatedBy = "System", CreatedDate = DateTime.UtcNow },
                
                // Vegetables
                new() { ProductCode = "202505-007", Name = "Carrot", Description = "Crunchy carrot", CategoryId = vegetablesCategoryId, Price = 10.00m, CreatedBy = "System", CreatedDate = DateTime.UtcNow },
                new() { ProductCode = "202505-008", Name = "Broccoli", Description = "Green broccoli", CategoryId = vegetablesCategoryId, Price = 50.00m, CreatedBy = "System", CreatedDate = DateTime.UtcNow },
                new() { ProductCode = "202505-009", Name = "Spinach", Description = "Fresh spinach", CategoryId = vegetablesCategoryId, Price = 20.00m, CreatedBy = "System", CreatedDate = DateTime.UtcNow },
                new() { ProductCode = "202505-010", Name = "Potato", Description = "Versatile potato", CategoryId = vegetablesCategoryId, Price = 100.00m, CreatedBy = "System", CreatedDate = DateTime.UtcNow },
                new() { ProductCode = "202505-011", Name = "Tomato", Description = "Ripe tomato", CategoryId = vegetablesCategoryId, Price = 30.00m, CreatedBy = "System", CreatedDate = DateTime.UtcNow },

            };
            context.Products.AddRange(products);
            context.SaveChanges();
        }
    }
}
