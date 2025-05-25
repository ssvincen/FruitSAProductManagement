using FruitSA.Application.Shared;
using FruitSA.Application.Shared.Product;
using FruitSA.Domain.Helper;
using MediatR;

namespace FruitSA.Application.Products.Queries.GetAllProductsByPagination
{
    public class GetAllProductsByPaginationQuery(PaginationModel pagination) : IRequest<PagedResult<ProductViewModel>>
    {
        public int PageNumber { get; set; } = pagination.PageNumber;
        public int PageSize { get; set; } = pagination.PageSize;
    }
}
