using FruitSA.Application.Interfaces;
using FruitSA.Application.Shared.Product;
using FruitSA.Domain.Helper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FruitSA.Application.Products.Queries.GetAllProductsByPagination
{
    public class GetAllProductsByPaginationHandler(IProductRepository productRepository)
        : IRequestHandler<GetAllProductsByPaginationQuery, PagedResult<ProductViewModel>>
    {
        public async Task<PagedResult<ProductViewModel>> Handle(GetAllProductsByPaginationQuery request, CancellationToken cancellationToken)
        {
            return await productRepository.GetAllProductsByPaginationAsync(request.PageNumber, request.PageSize, cancellationToken);
        }
    }
}
