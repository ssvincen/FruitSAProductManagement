using FruitSA.Domain.Helper;
using MediatR;

namespace FruitSA.Application.Categories.Commands.DeleteCategory
{
    public class DeleteCategoryCommand : IRequest<Result<bool>>
    {
        public int Id { get; set; }
    }
}
