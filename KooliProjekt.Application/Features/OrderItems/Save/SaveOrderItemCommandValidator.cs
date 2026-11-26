using System;
using System.Linq;
using FluentValidation;
using KooliProjekt.Application.Data;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.OrderItems
{
    public class SaveOrderItemCommandValidator : AbstractValidator<SaveOrderItemCommand>
    {
        private static readonly string[] ClosedStatuses = { "Completed", "Cancelled" };

        public SaveOrderItemCommandValidator(ApplicationDbContext dbContext)
        {
            // Id = 0 -> uus kirje, Id > 0 -> olemasoleva muutmine
            RuleFor(x => x.Id)
                .Cascade(CascadeMode.Stop)
                .GreaterThanOrEqualTo(0).WithMessage("Id cannot be negative")
                .MustAsync((id, ct) => dbContext.OrderItems.AnyAsync(e => e.Id == id, ct))
                .When(x => x.Id > 0, ApplyConditionTo.CurrentValidator)
                .WithMessage(x => "Cannot find order item with ID " + x.Id);

            RuleFor(x => x.OrderId)
                .Cascade(CascadeMode.Stop)
                .GreaterThan(0).WithMessage("Order is required")
                .MustAsync((orderId, ct) => dbContext.Orders.AnyAsync(o => o.Id == orderId, ct))
                .WithMessage(x => "Cannot find order with ID " + x.OrderId)
                .MustAsync(async (orderId, ct) =>
                    !await dbContext.Orders.AnyAsync(o => o.Id == orderId && ClosedStatuses.Contains(o.Status), ct))
                .WithMessage("Cannot change items of a completed or cancelled order");

            RuleFor(x => x.ProductId)
                .Cascade(CascadeMode.Stop)
                .GreaterThan(0).WithMessage("Product is required")
                .MustAsync((productId, ct) => dbContext.Products.AnyAsync(p => p.Id == productId, ct))
                .WithMessage(x => "Cannot find product with ID " + x.ProductId)
                .MustAsync(async (command, productId, ct) =>
                    !await dbContext.OrderItems.AnyAsync(i => i.Order.Id == command.OrderId
                                                           && i.Product.Id == productId
                                                           && i.Id != command.Id, ct))
                .WithMessage("This product is already in the order, change the quantity of the existing row instead");

            RuleFor(x => x.Quantity)
                .InclusiveBetween(1, 1000).WithMessage("Quantity must be between 1 and 1000");
        }
    }
}
