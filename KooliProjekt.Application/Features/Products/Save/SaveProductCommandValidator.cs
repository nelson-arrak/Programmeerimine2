using FluentValidation;

namespace KooliProjekt.Application.Features.Products
{
    public class SaveProductCommandValidator : AbstractValidator<SaveProductCommand>
    {
        public SaveProductCommandValidator()
        {
            RuleFor(command => command.Id)
                .GreaterThanOrEqualTo(0).WithMessage("Id cannot be negative");

            RuleFor(command => command.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(100).WithMessage("Name cannot be longer than 100 characters");

            RuleFor(command => command.Description)
                .NotEmpty().WithMessage("Description is required");

            RuleFor(command => command.Price)
                .InclusiveBetween(0.01m, 999999999m).WithMessage("Price must be between 0.01 and 999999999");

            RuleFor(command => command.StockQuantity)
                .GreaterThanOrEqualTo(0).WithMessage("Stock quantity cannot be negative");

            RuleFor(command => command.CategoryId)
                .GreaterThan(0).WithMessage("Category is required");
        }
    }
}
