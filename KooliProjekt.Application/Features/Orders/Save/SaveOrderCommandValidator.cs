using System.Linq;
using FluentValidation;

namespace KooliProjekt.Application.Features.Orders
{
    public class SaveOrderCommandValidator : AbstractValidator<SaveOrderCommand>
    {
        public static readonly string[] AllowedStatuses = { "Pending", "Paid", "Shipped", "Completed", "Cancelled" };

        public SaveOrderCommandValidator()
        {
            RuleFor(command => command.Id)
                .GreaterThanOrEqualTo(0).WithMessage("Id cannot be negative");

            RuleFor(command => command.CustomerId)
                .GreaterThan(0).WithMessage("Customer is required");

            RuleFor(command => command.OrderDate)
                .NotEmpty().WithMessage("Order date is required");

            RuleFor(command => command.Status)
                .NotEmpty().WithMessage("Status is required")
                .Must(status => AllowedStatuses.Contains(status))
                .When(command => !string.IsNullOrEmpty(command.Status))
                .WithMessage("Status must be one of: " + string.Join(", ", AllowedStatuses));
        }
    }
}
