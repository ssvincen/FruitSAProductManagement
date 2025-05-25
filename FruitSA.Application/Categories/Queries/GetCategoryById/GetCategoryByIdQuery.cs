using FruitSA.Application.Shared.Category;
using FruitSA.Domain.Helper;
using MediatR;

namespace FruitSA.Application.Categories.Queries.GetCategoryById
{
    public class GetCategoryByIdQuery(int id) : IRequest<Result<CategoryViewModel>>
    {
        public int Id { get; set; } = id;
    }
}
