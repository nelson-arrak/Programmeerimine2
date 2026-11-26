using System;
using System.Linq;
using FluentValidation;
using KooliProjekt.Application.Data;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Orders
{
    public class SaveOrderCommandValidator : AbstractValidator<SaveOrderCommand>
    {
        public static readonly string[] AllowedStatuses = { "Pending", "Paid", "Shipped", "Completed", "Cancelled" };

        public SaveOrderCommandValidator(ApplicationDbContext dbContext)
        {
            // Id = 0 -> uus kirje, Id > 0 -> olemasoleva muutmine
            RuleFor(x => x.Id)
                .Cascade(CascadeMode.Stop)
                .GreaterThanOrEqualTo(0).WithMessage("Id cannot be negative")
                .MustAsync((id, ct) => dbContext.Orders.AnyAsync(e => e.Id == id, ct))
                .When(x => x.Id > 0, ApplyConditionTo.CurrentValidator)
                .WithMessage(x => "Cannot find order with ID " + x.Id);

            RuleFor(x => x.CustomerId)
                .Cascade(CascadeMode.Stop)
                .GreaterThan(0).WithMessage("Customer is required")
                .MustAsync((customerId, ct) => dbContext.Customers.AnyAsync(c => c.Id == customerId, ct))
                .WithMessage(x => "Cannot find customer with ID " + x.CustomerId);

            RuleFor(x => x.OrderDate)
                .NotEmpty().WithMessage("Order date is required")
                .LessThanOrEqualTo(x => DateTime.Now).WithMessage("Order date cannot be in the future");

            RuleFor(x => x.Status)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Status is required")
                .Must(status => AllowedStatuses.Contains(status))
                .WithMessage("Status must be one of: " + string.Join(", ", AllowedStatuses));
        }
    }
}
