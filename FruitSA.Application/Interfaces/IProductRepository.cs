using FruitSA.Application.Shared.Product;
using FruitSA.Domain.Entities;
using FruitSA.Domain.Helper;
using System.Threading;
using System.Threading.Tasks;

namespace FruitSA.Application.Interfaces
{
    public interface IProductRepository
    {
        /// <summary>
        /// Retrieves a paged list of products, including their associated categories.
        /// </summary>
        /// <param name="pageNumber">The page number (1-based).</param>
        /// <param name="pageSize">The number of products per page (default 10).</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A paged result containing products and pagination metadata.</returns>
        Task<PagedResult<ProductViewModel>> GetAllProductsByPaginationAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);

        /// <summary>
        /// Retrieves a product by its ID.
        /// </summary>
        /// <param name="id">The product ID.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The product, or null if not found.</returns>
        Task<Result<ProductViewModel>> GetProductByIdAsync(int id, CancellationToken cancellationToken);

        /// <summary>
        /// Adds a new product to the database.
        /// </summary>
        /// <param name="product">The product to add.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The result of the add operation.</returns>
        Task<Result<ProductViewModel>> AddProductAsync(Product product, CancellationToken cancellationToken);

        /// <summary>
        /// Updates an existing product.
        /// </summary>
        /// <param name="product">The product with updated values.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The result of the update operation.</returns>
        Task<Result<ProductViewModel>> UpdateProductAsync(Product product, CancellationToken cancellationToken);

        /// <summary>
        /// Deletes a product by its ID.
        /// </summary>
        /// <param name="id">The product ID.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The result of the delete operation.</returns>
        Task<Result<bool>> DeleteProductAsync(int id, CancellationToken cancellationToken);
    }
}
