using FruitSA.Application.Shared;
using FruitSA.Application.Shared.Category;
using FruitSA.Domain.Helper;
using MediatR;

namespace FruitSA.Application.Categories.Queries.GetAllCategoriesByPagination
{
    public class GetAllCategoriesByPaginationQuery(PaginationModel pagination) : IRequest<PagedResult<CategoryViewModel>>
    {
        public int PageNumber { get; set; } = pagination.PageNumber;
        public int PageSize { get; set; } = pagination.PageSize;
    }
}
