using System;
using System.Linq;
using FluentValidation;
using KooliProjekt.Application.Data;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Products
{
    public class SaveProductCommandValidator : AbstractValidator<SaveProductCommand>
    {

        public SaveProductCommandValidator(ApplicationDbContext dbContext)
        {
            // Id = 0 -> uus kirje, Id > 0 -> olemasoleva muutmine
            RuleFor(x => x.Id)
                .Cascade(CascadeMode.Stop)
                .GreaterThanOrEqualTo(0).WithMessage("Id cannot be negative")
                .MustAsync((id, ct) => dbContext.Products.AnyAsync(e => e.Id == id, ct))
                .When(x => x.Id > 0, ApplyConditionTo.CurrentValidator)
                .WithMessage(x => "Cannot find product with ID " + x.Id);

            RuleFor(x => x.Name)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(100).WithMessage("Name cannot be longer than 100 characters")
                .MustAsync(async (command, name, ct) =>
                    !await dbContext.Products.AnyAsync(p => p.Name == name && p.Id != command.Id, ct))
                .WithMessage("Product with this name already exists");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required")
                .MaximumLength(2000).WithMessage("Description cannot be longer than 2000 characters");

            RuleFor(x => x.Price)
                .InclusiveBetween(0.01m, 999999999m).WithMessage("Price must be between 0.01 and 999999999")
                .PrecisionScale(11, 2, true).WithMessage("Price can have at most 2 decimal places");

            RuleFor(x => x.StockQuantity)
                .GreaterThanOrEqualTo(0).WithMessage("Stock quantity cannot be negative");

            RuleFor(x => x.CategoryId)
                .Cascade(CascadeMode.Stop)
                .GreaterThan(0).WithMessage("Category is required")
                .MustAsync((categoryId, ct) => dbContext.Category.AnyAsync(c => c.Id == categoryId, ct))
                .WithMessage(x => "Cannot find category with ID " + x.CategoryId);
        }
    }
}
