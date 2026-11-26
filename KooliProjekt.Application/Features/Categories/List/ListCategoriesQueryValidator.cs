using FluentValidation;

namespace KooliProjekt.Application.Features.Categories
{
    public class ListCategoriesQueryValidator : AbstractValidator<ListCategoriesQuery>
    {
        public ListCategoriesQueryValidator()
        {
            RuleFor(x => x.Page)
                .GreaterThanOrEqualTo(1).WithMessage("Page must be at least 1");

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 100).WithMessage("Page size must be between 1 and 100");
        }
    }
}
