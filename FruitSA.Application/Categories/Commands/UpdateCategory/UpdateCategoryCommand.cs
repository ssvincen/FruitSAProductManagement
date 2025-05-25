using FruitSA.Application.Shared.Category;
using FruitSA.Domain.Helper;
using MediatR;
using System;

namespace FruitSA.Application.Categories.Commands.UpdateCategory
{
    public class UpdateCategoryCommand : IRequest<Result<CategoryViewModel>>
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CategoryCode { get; set; }
        public bool IsActive { get; set; } = true;
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
