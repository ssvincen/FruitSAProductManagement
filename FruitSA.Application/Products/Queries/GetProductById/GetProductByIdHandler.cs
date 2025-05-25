using FruitSA.Application.Interfaces;
using FruitSA.Application.Shared.Product;
using FruitSA.Domain.Helper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FruitSA.Application.Products.Queries.GetProductById
{
    public class GetProductByIdHandler(IProductRepository productRepository)
        : IRequestHandler<GetProductByIdQuery, Result<ProductViewModel>>
    {
        private readonly IProductRepository _productRepository = productRepository;
        public async Task<Result<ProductViewModel>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            return await _productRepository.GetProductByIdAsync(request.Id, cancellationToken);
        }
    }
}
