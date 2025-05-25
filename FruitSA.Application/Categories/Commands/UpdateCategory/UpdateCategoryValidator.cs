using FluentValidation;

namespace FruitSA.Application.Categories.Commands.UpdateCategory
{
    public class UpdateCategoryValidator : AbstractValidator<UpdateCategoryCommand>
    {
        public UpdateCategoryValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name must be at most 100 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(255).WithMessage("Description must be at most 255 characters.")
                .When(x => !string.IsNullOrEmpty(x.Description));

            RuleFor(x => x.CategoryCode)
                .NotEmpty().WithMessage("Category Code is required.")
                .MaximumLength(6).WithMessage("Category Code must be at most 6 characters.")
                .Matches(@"^[A-Z]{3}\d{3}$")
                .WithMessage("Category Code must be 3 letters followed by 3 numbers (e.g., ABC123).");

            RuleFor(x => x.CreatedBy)
                .NotEmpty().WithMessage("Created By is required.");

        }
    }
}
