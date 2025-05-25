using FruitSA.Application.Shared.Product;
using FruitSA.Domain.Helper;
using MediatR;

namespace FruitSA.Application.Products.Commands.AddProduct
{
    public class AddProductCommand : IRequest<Result<ProductViewModel>>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public decimal Price { get; set; }
        public string ImagePath { get; set; }
        public string CreatedBy { get; set; }
    }
}
