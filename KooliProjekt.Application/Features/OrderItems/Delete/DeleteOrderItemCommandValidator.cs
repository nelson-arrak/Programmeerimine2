using FluentValidation;

namespace KooliProjekt.Application.Features.OrderItems
{
    public class DeleteOrderItemCommandValidator : AbstractValidator<DeleteOrderItemCommand>
    {
        public DeleteOrderItemCommandValidator()
        {
            RuleFor(command => command.Id)
                .GreaterThan(0).WithMessage("Id must be greater than 0");
        }
    }
}
