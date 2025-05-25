using FruitSA.Application.Interfaces;
using FruitSA.Application.Shared.Category;
using FruitSA.Domain.Helper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FruitSA.Application.Categories.Queries.GetAllCategoriesByPagination
{
    public class GetAllCategoriesByPaginationHandler(ICategoryRepository categoryRepository)
        : IRequestHandler<GetAllCategoriesByPaginationQuery, PagedResult<CategoryViewModel>>
    {
        private readonly ICategoryRepository _categoryRepository = categoryRepository;

        public async Task<PagedResult<CategoryViewModel>> Handle(GetAllCategoriesByPaginationQuery request, CancellationToken cancellationToken)
        {
            return await _categoryRepository.GetAllCategoriesByPaginationAsync(request.PageNumber, request.PageSize, cancellationToken);
        }
    }
}
