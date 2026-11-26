using FluentValidation;
using KooliProjekt.Application.Infrastructure.Files;

namespace KooliProjekt.Application.Features.Products
{
    public class DeleteFileCommandValidator : AbstractValidator<DeleteFileCommand>
    {
        public DeleteFileCommandValidator(IFileClient fileClient)
        {
            RuleFor(x => x.ProductId)
                .GreaterThan(0).WithMessage("Product ID must be greater than 0");

            RuleFor(x => x.FileName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("File name is required")
                .Must(ProductFileRules.IsPlainFileName).WithMessage("File name is not valid")
                .MustAsync((command, fileName, ct) => fileClient.Exists(fileName, FileStore.ProductPhotos(command.ProductId)))
                .WithMessage(x => "Cannot find file " + x.FileName + " for product " + x.ProductId)
                .When(x => x.ProductId > 0, ApplyConditionTo.CurrentValidator);
        }
    }
}
