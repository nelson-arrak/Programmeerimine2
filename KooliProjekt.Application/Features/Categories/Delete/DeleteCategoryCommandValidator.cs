using FluentValidation;
using KooliProjekt.Application.Data;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Categories
{
    public class DeleteCategoryCommandValidator : AbstractValidator<DeleteCategoryCommand>
    {
        public DeleteCategoryCommandValidator(ApplicationDbContext dbContext)
        {
            RuleFor(x => x.Id)
                .Cascade(CascadeMode.Stop)
                .GreaterThan(0).WithMessage("Id must be greater than 0")
                .MustAsync((id, ct) => dbContext.Category.AnyAsync(e => e.Id == id, ct))
                .WithMessage(x => "Cannot find category with ID " + x.Id)
                .MustAsync(async (id, ct) => !await dbContext.Products.AnyAsync(p => p.Category.Id == id, ct))
                .WithMessage("Cannot delete category because it has products");
        }
    }
}
