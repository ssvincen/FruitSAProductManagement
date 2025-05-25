using FruitSA.Application.Interfaces;
using FruitSA.Application.Shared.Category;
using FruitSA.Domain.Entities;
using FruitSA.Domain.Helper;
using FruitSA.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FruitSA.Infrastructure.Repositories
{
    public class CategoryRepository(ApplicationDbContext context, ILogger<CategoryRepository> logger) : ICategoryRepository
    {
        private readonly ApplicationDbContext _context = context;
        private readonly ILogger<CategoryRepository> _logger = logger;

        public async Task<PagedResult<CategoryViewModel>> GetAllCategoriesByPaginationAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            var query = _context.Categories.AsNoTracking().OrderBy(c => c.Name);
            var totalRecords = await query.CountAsync(cancellationToken);
            var items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize)
                 .Select(c => new CategoryViewModel
                 {
                     CategoryId = c.CategoryId,
                     Name = c.Name,
                     Description = c.Description,
                     CategoryCode = c.CategoryCode,
                     IsActive = c.IsActive,
                     CreatedBy = c.CreatedBy,
                     CreatedDate = c.CreatedDate
                 }).ToListAsync(cancellationToken);

            return new PagedResult<CategoryViewModel>
            {
                Items = items,
                TotalCount = totalRecords,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<List<CategoryViewModel>> GetAllCategoriesAsync(CancellationToken cancellationToken)
        {
            var response = await _context.Categories.AsNoTracking().OrderBy(c => c.Name)
                .Select(c => new CategoryViewModel
                {
                    CategoryId = c.CategoryId,
                    Name = c.Name,
                    Description = c.Description,
                    CategoryCode = c.CategoryCode,
                    IsActive = c.IsActive,
                    CreatedBy = c.CreatedBy,
                    CreatedDate = c.CreatedDate
                }).ToListAsync(cancellationToken);

            return response;
        }

        public async Task<Result<CategoryViewModel>> GetCategoryByIdAsync(int id, CancellationToken cancellationToken)
        {
            try
            {
                var category = await _context.Categories.AsNoTracking()
                    .FirstOrDefaultAsync(c => c.CategoryId == id, cancellationToken);

                if (category == null)
                    return Result<CategoryViewModel>.Fail("Category not found.");

                return Result<CategoryViewModel>.Ok(MapToViewModel(category));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving category by ID.");
                return Result<CategoryViewModel>.Fail("An unexpected error occurred.");
            }
        }

        public async Task<Result<CategoryViewModel>> AddCategoryAsync(Category category, CancellationToken cancellationToken)
        {
            if (category == null)
                return Result<CategoryViewModel>.Fail("Category is null.");

            try
            {
                await _context.Categories.AddAsync(category, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                return Result<CategoryViewModel>.Ok(MapToViewModel(category), "Category added successfully.");
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Database error while adding category.");
                return Result<CategoryViewModel>.Fail("A database error occurred while adding the category.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while adding category.");
                return Result<CategoryViewModel>.Fail("An unexpected error occurred.");
            }
        }

        public async Task<Result<CategoryViewModel>> UpdateCategoryAsync(Category category, CancellationToken cancellationToken)
        {
            try
            {
                var existing = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryId == category.CategoryId, cancellationToken);
                if (existing == null)
                    return Result<CategoryViewModel>.Fail("Category not found.");

                existing.Name = category.Name;
                existing.CategoryCode = category.CategoryCode;
                existing.IsActive = category.IsActive;
                existing.Description = category.Description;

                _context.Categories.Update(existing);
                await _context.SaveChangesAsync(cancellationToken);

                return Result<CategoryViewModel>.Ok(MapToViewModel(existing), "Category updated successfully.");
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Database error while updating category.");
                return Result<CategoryViewModel>.Fail("A database error occurred while updating the category.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while updating category.");
                return Result<CategoryViewModel>.Fail("An unexpected error occurred.");
            }
        }

        public async Task<Result<bool>> DeleteCategoryByIdAsync(int id, CancellationToken cancellationToken)
        {
            try
            {
                var category = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryId == id, cancellationToken);
                if (category == null)
                    return Result<bool>.Fail("Category not found.");

                _context.Categories.Remove(category);
                await _context.SaveChangesAsync(cancellationToken);

                return Result<bool>.Ok(true, "Category deleted successfully.");
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Database error while deleting category.");
                return Result<bool>.Fail("A database error occurred while deleting the category.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while deleting category.");
                return Result<bool>.Fail("An unexpected error occurred.");
            }
        }

        private static CategoryViewModel MapToViewModel(Category category)
        {
            return new CategoryViewModel
            {
                CategoryId = category.CategoryId,
                Name = category.Name,
                Description = category.Description,
                CategoryCode = category.CategoryCode,
                IsActive = category.IsActive,
                CreatedBy = category.CreatedBy,
                CreatedDate = category.CreatedDate
            };
        }

    }
}
