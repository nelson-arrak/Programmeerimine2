using FluentValidation;

namespace KooliProjekt.Application.Features.OrderItems
{
    public class SaveOrderItemCommandValidator : AbstractValidator<SaveOrderItemCommand>
    {
        public SaveOrderItemCommandValidator()
        {
            RuleFor(command => command.Id)
                .GreaterThanOrEqualTo(0).WithMessage("Id cannot be negative");

            RuleFor(command => command.OrderId)
                .GreaterThan(0).WithMessage("Order is required");

            RuleFor(command => command.ProductId)
                .GreaterThan(0).WithMessage("Product is required");

            RuleFor(command => command.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be at least 1");
        }
    }
}
