using FruitSA.Application.Interfaces;
using FruitSA.Domain.Entities;
using FruitSA.Domain.Helper;
using FruitSA.Infrastructure.Data;
using Microsoft.Extensions.Logging;

namespace FruitSA.Infrastructure.Repositories
{
    public class CategoryRepository(ApplicationDbContext context, ILogger<CategoryRepository> logger) : ICategoryRepository
    {
        private readonly ApplicationDbContext _context = context;
        private readonly ILogger<CategoryRepository> _logger = logger;


        public Task<PagedResult<Category>> GetAllCategoriesByPaginationAsync(int pageNumber, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Task AddCategoryAsync(Category category)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteCategoryAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Category>> GetAllCategoriesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Category> GetCategoryByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Category> UpdateCategoryAsync(Category category)
        {
            throw new NotImplementedException();
        }
    }
}
