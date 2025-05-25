using FruitSA.Domain.Helper;
using MediatR;

namespace FruitSA.Application.Products.Commands.DeleteProduct
{
    public class DeleteProductCommand(int id) : IRequest<Result<bool>>
    {
        public int Id { get; set; } = id;
    }
}
