using FruitSA.Domain.Entities;
using FruitSA.Domain.Helper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FruitSA.Application.Interfaces
{
    public interface ICategoryRepository
    {
        /// <summary>
        /// Retrieves a paged list of categories.
        /// </summary>
        /// <param name="page">The page number (1-based).</param>
        /// <param name="pageSize">The number of categories per page (default 10).</param>
        /// <returns>A paged result containing categories and pagination metadata.</returns>
        Task<PagedResult<Category>> GetAllCategoriesByPaginationAsync(int pageNumber, int pageSize);

        /// <summary>
        /// Retrieves all categories for dropdowns or lists (non-paged).
        /// </summary>
        /// <returns>A list of all categories.</returns>
        Task<List<Category>> GetAllCategoriesAsync();


        /// <summary>
        /// Retrieves a category by its ID.
        /// </summary>
        /// <param name="id">The category ID.</param>
        /// <returns>The category, or null if not found.</returns>
        Task<Category> GetCategoryByIdAsync(int id);


        /// <summary>
        /// Adds a new category to the database.
        /// </summary>
        /// <param name="category">The category to add.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task AddCategoryAsync(Category category);


        /// <summary>
        /// Updates an existing category.
        /// </summary>
        /// <param name="category">The category with updated values.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task<Category> UpdateCategoryAsync(Category category);


        /// <summary>
        /// Deletes a category by its ID, if no products are associated.
        /// </summary>
        /// <param name="id">The category ID.</param>
        /// <returns>True if deleted, false if not found or has associated products.</returns>
        Task<bool> DeleteCategoryAsync(int id);
    }
}
