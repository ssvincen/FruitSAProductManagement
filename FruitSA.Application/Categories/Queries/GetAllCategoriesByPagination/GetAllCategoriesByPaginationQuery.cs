using FruitSA.Application.Shared;
using FruitSA.Application.Shared.Category;
using FruitSA.Domain.Helper;
using MediatR;

namespace FruitSA.Application.Categories.Queries.GetAllCategoriesByPagination
{
    public class GetAllCategoriesByPaginationQuery(PageModel pageModel) : IRequest<PagedResult<CategoryViewModel>>
    {
        public int PageNumber { get; set; } = pageModel.PageNumber;
        public int PageSize { get; set; } = pageModel.PageSize;
    }
}
