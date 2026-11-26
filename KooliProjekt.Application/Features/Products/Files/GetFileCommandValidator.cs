using FluentValidation;

namespace KooliProjekt.Application.Features.Products
{
    public class GetFileCommandValidator : AbstractValidator<GetFileCommand>
    {
        public GetFileCommandValidator()
        {
            RuleFor(x => x.ProductId)
                .GreaterThan(0).WithMessage("Product ID must be greater than 0");

            RuleFor(x => x.FileName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("File name is required")
                .Must(ProductFileRules.IsPlainFileName).WithMessage("File name is not valid");
        }
    }
}
