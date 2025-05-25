using FruitSA.Application.Interfaces;
using FruitSA.Application.Shared.Product;
using FruitSA.Domain.Entities;
using FruitSA.Domain.Helper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FruitSA.Application.Products.Commands.UpdateProduct
{
    public class UpdateProductHandler(IProductRepository productRepository)
        : IRequestHandler<UpdateProductCommand, Result<ProductViewModel>>
    {
        private readonly IProductRepository _productRepository = productRepository;

        public async Task<Result<ProductViewModel>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = new Product
            {
                ProductId = request.Id,
                Name = request.Name,
                Description = request.Description,
                CategoryId = request.CategoryId,
                Price = request.Price,
                ImagePath = request.ImagePath,
            };
            return await _productRepository.UpdateProductAsync(product, cancellationToken);
        }
    }
}
