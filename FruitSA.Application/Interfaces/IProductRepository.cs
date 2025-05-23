using FruitSA.Domain.Entities;
using FruitSA.Domain.Helper;
using System.Threading.Tasks;

namespace FruitSA.Application.Interfaces
{
    public interface IProductRepository
    {
        /// <summary>
        /// Retrieves a paged list of products, including their associated categories.
        /// </summary>
        /// <param name="page">The page number (1-based).</param>
        /// <param name="pageSize">The number of products per page (default 10).</param>
        /// <returns>A paged result containing products and pagination metadata.</returns>
        Task<PagedResult<Product>> GetAllProductsByPaginationAsync(int pageNumber, int pageSize);


        /// <summary>
        /// Retrieves a product by its ID.
        /// </summary>
        /// <param name="id">The product ID.</param>
        /// <returns>The product, or null if not found.</returns>
        Task<Product> GetProductByIdAsync(int id);

        /// <summary>
        /// Adds a new product to the database.
        /// </summary>
        /// <param name="product">The product to add.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task<Product> AddProductAsync(Product product);

        /// <summary>
        /// Updates an existing product.
        /// </summary>
        /// <param name="product">The product with updated values.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task<Product> UpdateProductAsync(Product product);

        /// <summary>
        /// Saves changes to the database.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task<bool> DeleteProductAsync(int id);
    }
}
