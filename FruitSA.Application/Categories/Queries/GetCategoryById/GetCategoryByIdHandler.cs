using FruitSA.Application.Interfaces;
using FruitSA.Application.Shared.Category;
using FruitSA.Domain.Helper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FruitSA.Application.Categories.Queries.GetCategoryById
{
    public class GetCategoryByIdHandler(ICategoryRepository categoryRepository) : IRequestHandler<GetCategoryByIdQuery, Result<CategoryViewModel>>
    {
        private readonly ICategoryRepository _categoryRepository = categoryRepository;

        public async Task<Result<CategoryViewModel>> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            return await _categoryRepository.GetCategoryByIdAsync(request.Id, cancellationToken);
        }
    }
}
