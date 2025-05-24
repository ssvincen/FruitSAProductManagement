using FruitSA.Application.Interfaces;
using FruitSA.Application.Shared.Category;
using FruitSA.Domain.Entities;
using FruitSA.Domain.Helper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FruitSA.Application.Categories.Commands.AddCategory
{
    public class AddCategoryHandler(ICategoryRepository categoryRepository)
        : IRequestHandler<AddCategoryCommand, Result<CategoryViewModel>>
    {
        private readonly ICategoryRepository _categoryRepository = categoryRepository;

        public async Task<Result<CategoryViewModel>> Handle(AddCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = new Category
            {
                Name = request.Name,
                Description = request.Description,
                CategoryCode = request.CategoryCode,
                CreatedBy = request.CreatedBy,
                IsActive = true,
            };
            return await _categoryRepository.AddCategoryAsync(category, cancellationToken);
        }
    }
}
