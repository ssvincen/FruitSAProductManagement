using FruitSA.Application.Interfaces;
using FruitSA.Application.Shared.Category;
using FruitSA.Domain.Entities;
using FruitSA.Domain.Helper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FruitSA.Application.Categories.Commands.UpdateCategory
{
    public class UpdateCategoryHandler(ICategoryRepository categoryRepository)
        : IRequestHandler<UpdateCategoryCommand, Result<CategoryViewModel>>
    {
        private readonly ICategoryRepository _categoryRepository = categoryRepository;
        public async Task<Result<CategoryViewModel>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var product = new Category()
            {
                CategoryId = request.CategoryId,
                Name = request.Name,
                CategoryCode = request.CategoryCode,
                Description = request.Description,
                IsActive = request.IsActive
            };
            return await _categoryRepository.UpdateCategoryAsync(product, cancellationToken);
        }
    }
}
