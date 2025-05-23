using FruitSA.Application.Interfaces;
using FruitSA.Domain.Entities;
using FruitSA.Domain.Helper;
using FruitSA.Infrastructure.Data;
using Microsoft.Extensions.Logging;

namespace FruitSA.Infrastructure.Repositories
{
    public class ProductRepository(ApplicationDbContext context, ILogger<ProductRepository> logger) : IProductRepository
    {
        private readonly ApplicationDbContext _context = context;
        private readonly ILogger<ProductRepository> _logger = logger;

        public Task<Product> AddProductAsync(Product product)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteProductAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResult<Product>> GetAllProductsByPaginationAsync(int pageNumber, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Task<Product> GetProductByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Product> UpdateProductAsync(Product product)
        {
            throw new NotImplementedException();
        }
    }
}
