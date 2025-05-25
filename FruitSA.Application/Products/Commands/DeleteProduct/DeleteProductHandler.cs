using FruitSA.Application.Interfaces;
using FruitSA.Domain.Helper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FruitSA.Application.Products.Commands.DeleteProduct
{
    public class DeleteProductHandler(IProductRepository productRepository)
        : IRequestHandler<DeleteProductCommand, Result<bool>>
    {
        private readonly IProductRepository _productRepository = productRepository;

        public async Task<Result<bool>> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            return await _productRepository.DeleteProductByIdAsync(request.Id, cancellationToken);
        }
    }
}
