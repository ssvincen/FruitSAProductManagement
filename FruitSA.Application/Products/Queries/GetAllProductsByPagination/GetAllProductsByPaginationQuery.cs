using FruitSA.Application.Shared;
using FruitSA.Application.Shared.Product;
using FruitSA.Domain.Helper;
using MediatR;

namespace FruitSA.Application.Products.Queries.GetAllProductsByPagination
{
    public class GetAllProductsByPaginationQuery(PageModel pageModel) : IRequest<PagedResult<ProductViewModel>>
    {
        public int PageNumber { get; set; } = pageModel.PageNumber;
        public int PageSize { get; set; } = pageModel.PageSize;
    }
}
