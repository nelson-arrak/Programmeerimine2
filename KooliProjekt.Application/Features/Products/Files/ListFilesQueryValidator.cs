using FluentValidation;

namespace KooliProjekt.Application.Features.Products
{
    public class ListFilesQueryValidator : AbstractValidator<ListFilesQuery>
    {
        public ListFilesQueryValidator()
        {
            RuleFor(x => x.ProductId)
                .GreaterThan(0).WithMessage("Product ID must be greater than 0");
        }
    }
}
