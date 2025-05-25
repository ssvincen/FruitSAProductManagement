using FruitSA.Application.Interfaces;
using FruitSA.Application.Shared.Product;
using FruitSA.Domain.Entities;
using FruitSA.Domain.Helper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FruitSA.Application.Products.Commands.AddProduct
{
    public class AddProductHandler(IProductRepository productRepository)
        : IRequestHandler<AddProductCommand, Result<ProductViewModel>>
    {
        private readonly IProductRepository _productRepository = productRepository;
        public async Task<Result<ProductViewModel>> Handle(AddProductCommand request, CancellationToken cancellationToken)
        {
            var product = new Product
            {
                Name = request.Name,
                Description = request.Description,
                CategoryId = request.CategoryId,
                Price = request.Price,
                ImagePath = request.ImagePath,
                CreatedBy = request.CreatedBy,
            };
            return await _productRepository.AddProductAsync(product, cancellationToken);
        }
    }
}
