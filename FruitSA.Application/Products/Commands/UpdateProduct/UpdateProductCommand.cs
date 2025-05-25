using FruitSA.Application.Shared.Product;
using FruitSA.Domain.Helper;
using MediatR;

namespace FruitSA.Application.Products.Commands.UpdateProduct
{
    public class UpdateProductCommand : IRequest<Result<ProductViewModel>>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public decimal Price { get; set; }
        public string ImagePath { get; set; }
    }
}
