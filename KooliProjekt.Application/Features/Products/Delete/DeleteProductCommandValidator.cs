using FluentValidation;
using KooliProjekt.Application.Data;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Products
{
    public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
    {
        public DeleteProductCommandValidator(ApplicationDbContext dbContext)
        {
            RuleFor(x => x.Id)
                .Cascade(CascadeMode.Stop)
                .GreaterThan(0).WithMessage("Id must be greater than 0")
                .MustAsync((id, ct) => dbContext.Products.AnyAsync(e => e.Id == id, ct))
                .WithMessage(x => "Cannot find product with ID " + x.Id)
                .MustAsync(async (id, ct) => !await dbContext.OrderItems.AnyAsync(i => i.Product.Id == id, ct))
                .WithMessage("Cannot delete product because it is used in orders");
        }
    }
}
