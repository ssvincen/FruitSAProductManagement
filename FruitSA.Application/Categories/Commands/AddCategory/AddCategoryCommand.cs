using FruitSA.Application.Shared.Category;
using FruitSA.Domain.Helper;
using MediatR;

namespace FruitSA.Application.Categories.Commands.AddCategory
{
    public class AddCategoryCommand : IRequest<Result<CategoryViewModel>>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string CategoryCode { get; set; }
        public string CreatedBy { get; set; }

    }
}
