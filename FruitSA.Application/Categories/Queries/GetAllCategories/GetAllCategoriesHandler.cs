using FruitSA.Application.Interfaces;
using FruitSA.Application.Shared.Category;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FruitSA.Application.Categories.Queries.GetAllCategories
{
    public class GetAllCategoriesHandler(ICategoryRepository categoryRepository)
        : IRequestHandler<GetAllCategoriesQuery, List<CategoryViewModel>>
    {
        private readonly ICategoryRepository _categoryRepository = categoryRepository;
        public async Task<List<CategoryViewModel>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            return await _categoryRepository.GetAllCategoriesAsync(cancellationToken);
        }
    }
}
