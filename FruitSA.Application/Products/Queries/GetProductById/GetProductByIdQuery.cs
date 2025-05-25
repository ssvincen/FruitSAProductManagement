using FruitSA.Application.Shared.Product;
using FruitSA.Domain.Helper;
using MediatR;

namespace FruitSA.Application.Products.Queries.GetProductById
{
    public class GetProductByIdQuery(int id) : IRequest<Result<ProductViewModel>>
    {
        public int Id { get; set; } = id;
    }
}
