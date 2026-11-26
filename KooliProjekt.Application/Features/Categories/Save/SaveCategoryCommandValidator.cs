using FluentValidation;

namespace KooliProjekt.Application.Features.Categories
{
    public class SaveCategoryCommandValidator : AbstractValidator<SaveCategoryCommand>
    {
        public SaveCategoryCommandValidator()
        {
            RuleFor(command => command.Id)
                .GreaterThanOrEqualTo(0).WithMessage("Id cannot be negative");

            RuleFor(command => command.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(50).WithMessage("Name cannot be longer than 50 characters");
        }
    }
}
