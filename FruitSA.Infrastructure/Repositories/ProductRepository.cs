using FruitSA.Application.Interfaces;
using FruitSA.Application.Shared.Product;
using FruitSA.Domain.Entities;
using FruitSA.Domain.Helper;
using FruitSA.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FruitSA.Infrastructure.Repositories
{
    public class ProductRepository(ApplicationDbContext context, ILogger<ProductRepository> logger) : IProductRepository
    {
        private readonly ApplicationDbContext _context = context;
        private readonly ILogger<ProductRepository> _logger = logger;

        public async Task<PagedResult<ProductViewModel>> GetAllProductsByPaginationAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            var query = _context.Products.AsNoTracking().Include(p => p.Category).OrderBy(p => p.Name);
            var totalRecords = await query.CountAsync(cancellationToken);
            var products = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);
            var items = products.Select(MapToViewModel).ToList();

            return new PagedResult<ProductViewModel>
            {
                Items = items,
                TotalCount = totalRecords,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<Result<ProductViewModel>> GetProductByIdAsync(int id, CancellationToken cancellationToken)
        {
            try
            {
                var product = await _context.Products.Include(p => p.Category)
                    .FirstOrDefaultAsync(p => p.ProductId == id, cancellationToken);

                if (product == null)
                    return Result<ProductViewModel>.Fail("Product not found.");

                return Result<ProductViewModel>.Ok(MapToViewModel(product));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving product with ID {ProductId}", id);
                return Result<ProductViewModel>.Fail("An unexpected error occurred.");
            }
        }

        public async Task<Result<ProductViewModel>> AddProductAsync(Product product, CancellationToken cancellationToken)
        {
            if (product == null)
                return Result<ProductViewModel>.Fail("Product is null.");

            try
            {
                var monthPrefix = DateTime.UtcNow.ToString("yyyyMM");
                var count = await _context.Products.CountAsync(p => p.ProductCode.StartsWith(monthPrefix), cancellationToken);

                product.ProductCode = $"{monthPrefix}-{count + 1:000}";

                await _context.Products.AddAsync(product, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                // Reload product with Category included
                var createdProduct = await _context.Products
                    .Include(p => p.Category)
                    .FirstOrDefaultAsync(p => p.ProductId == product.ProductId, cancellationToken);

                return Result<ProductViewModel>.Ok(MapToViewModel(createdProduct), "Product added successfully.");
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Database error while adding product.");
                return Result<ProductViewModel>.Fail("A database error occurred while adding the product.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while adding product.");
                return Result<ProductViewModel>.Fail("An unexpected error occurred.");
            }
        }

        public async Task<Result<ProductViewModel>> UpdateProductAsync(Product product, CancellationToken cancellationToken)
        {
            if (product == null)
                return Result<ProductViewModel>.Fail("Product is null.");

            try
            {
                var existing = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == product.ProductId, cancellationToken);
                if (existing == null)
                    return Result<ProductViewModel>.Fail("Product not found.");

                existing.Name = product.Name;
                existing.Description = product.Description;
                existing.Price = product.Price;
                existing.CategoryId = product.CategoryId;

                _context.Products.Update(existing);
                await _context.SaveChangesAsync(cancellationToken);

                // Reload updated product with Category
                var updatedProduct = await _context.Products.Include(p => p.Category)
                    .FirstOrDefaultAsync(p => p.ProductId == existing.ProductId, cancellationToken);

                return Result<ProductViewModel>.Ok(MapToViewModel(updatedProduct), "Product updated successfully.");
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Database error while updating product.");
                return Result<ProductViewModel>.Fail("A database error occurred while updating the product.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while updating product.");
                return Result<ProductViewModel>.Fail("An unexpected error occurred.");
            }
        }

        public async Task<Result<bool>> DeleteProductByIdAsync(int id, CancellationToken cancellationToken)
        {
            try
            {
                var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == id, cancellationToken);
                if (product == null)
                    return Result<bool>.Fail("Product not found.");

                _context.Products.Remove(product);
                await _context.SaveChangesAsync(cancellationToken);

                return Result<bool>.Ok(true, "Product deleted successfully.");
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Database error while deleting product.");
                return Result<bool>.Fail("A database error occurred while deleting the product.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while deleting product.");
                return Result<bool>.Fail("An unexpected error occurred.");
            }
        }

        private static ProductViewModel MapToViewModel(Product product)
        {
            return new ProductViewModel
            {
                ProductId = product.ProductId,
                ProductCode = product.ProductCode,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                CategoryName = product.Category.Name
            };
        }


    }
}
