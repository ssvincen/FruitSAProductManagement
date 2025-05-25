using FluentValidation;

namespace FruitSA.Application.Categories.Commands.DeleteCategory
{
    public class DeleteCategoryValidator : AbstractValidator<DeleteCategoryCommand>
    {
        public DeleteCategoryValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Category ID is required.");
        }
    }
}
