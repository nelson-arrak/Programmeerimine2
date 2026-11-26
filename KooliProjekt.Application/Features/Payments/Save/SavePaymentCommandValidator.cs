using FluentValidation;

namespace KooliProjekt.Application.Features.Payments
{
    public class SavePaymentCommandValidator : AbstractValidator<SavePaymentCommand>
    {
        public SavePaymentCommandValidator()
        {
            RuleFor(command => command.Id)
                .GreaterThanOrEqualTo(0).WithMessage("Id cannot be negative");

            RuleFor(command => command.OrderId)
                .GreaterThan(0).WithMessage("Order is required");

            RuleFor(command => command.Amount)
                .InclusiveBetween(0.01m, 999999999m).WithMessage("Amount must be between 0.01 and 999999999");

            RuleFor(command => command.PaymentDate)
                .NotEmpty().WithMessage("Payment date is required");

            RuleFor(command => command.PaymentMethod)
                .NotEmpty().WithMessage("Payment method is required")
                .MaximumLength(50).WithMessage("Payment method cannot be longer than 50 characters");
        }
    }
}
