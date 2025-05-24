using FruitSA.Application.Shared.Category;
using FruitSA.Domain.Entities;
using FruitSA.Domain.Helper;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FruitSA.Application.Interfaces
{
    public interface ICategoryRepository
    {
        /// <summary>
        /// Retrieves a paged list of categories.
        /// </summary>
        /// <param name="pageNumber">The page number (1-based).</param>
        /// <param name="pageSize">The number of categories per page.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A paged result containing categories and pagination metadata.</returns>
        Task<PagedResult<CategoryViewModel>> GetAllCategoriesByPaginationAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);

        /// <summary>
        /// Retrieves all categories for dropdowns or lists (non-paged).
        /// </summary>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A list of all categories.</returns>
        Task<List<CategoryViewModel>> GetAllCategoriesAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Retrieves a category by its ID.
        /// </summary>
        /// <param name="id">The category ID.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>The category, or null if not found.</returns>
        Task<Result<CategoryViewModel>> GetCategoryByIdAsync(int id, CancellationToken cancellationToken);

        /// <summary>
        /// Adds a new category to the database.
        /// </summary>
        /// <param name="category">The category to add.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>The result of the add operation.</returns>
        Task<Result<CategoryViewModel>> AddCategoryAsync(Category category, CancellationToken cancellationToken);

        /// <summary>
        /// Updates an existing category.
        /// </summary>
        /// <param name="category">The category with updated values.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>The result of the update operation.</returns>
        Task<Result<CategoryViewModel>> UpdateCategoryAsync(Category category, CancellationToken cancellationToken);

        /// <summary>
        /// Deletes a category by its ID, if no products are associated.
        /// </summary>
        /// <param name="id">The category ID.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>True if deleted, false if not found or has associated products.</returns>
        Task<Result<bool>> DeleteCategoryAsync(int id, CancellationToken cancellationToken);
    }
}
