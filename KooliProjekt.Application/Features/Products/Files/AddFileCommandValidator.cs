using FluentValidation;
using KooliProjekt.Application.Data;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Products
{
    public class AddFileCommandValidator : AbstractValidator<AddFileCommand>
    {
        public AddFileCommandValidator(ApplicationDbContext dbContext)
        {
            RuleFor(x => x.ProductId)
                .Cascade(CascadeMode.Stop)
                .GreaterThan(0).WithMessage("Product ID must be greater than 0")
                .MustAsync((id, ct) => dbContext.Products.AnyAsync(p => p.Id == id, ct))
                .WithMessage(x => "Cannot find product with ID " + x.ProductId);

            RuleFor(x => x.InputStream)
                .NotNull().WithMessage("File is required");

            RuleFor(x => x.FileName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("File name is required")
                .Must(ProductFileRules.HasAllowedExtension)
                .WithMessage("Only image files are allowed: " + string.Join(", ", ProductFileRules.AllowedExtensions))
                .When(x => x.InputStream != null);

            RuleFor(x => x.FileSize)
                .GreaterThan(0).WithMessage("File cannot be empty")
                .LessThanOrEqualTo(ProductFileRules.MaxFileSize).WithMessage("File cannot be larger than 5 MB")
                .When(x => x.InputStream != null);
        }
    }
}
