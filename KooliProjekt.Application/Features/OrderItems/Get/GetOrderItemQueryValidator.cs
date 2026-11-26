using FluentValidation;

namespace KooliProjekt.Application.Features.OrderItems
{
    public class GetOrderItemQueryValidator : AbstractValidator<GetOrderItemQuery>
    {
        public GetOrderItemQueryValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id must be greater than 0");
        }
    }
}
