using FluentValidation;

namespace KooliProjekt.Application.Features.Payments
{
    public class DeletePaymentCommandValidator : AbstractValidator<DeletePaymentCommand>
    {
        public DeletePaymentCommandValidator()
        {
            RuleFor(command => command.Id)
                .GreaterThan(0).WithMessage("Id must be greater than 0");
        }
    }
}
