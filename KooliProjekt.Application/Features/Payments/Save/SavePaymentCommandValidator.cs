using System;
using System.Linq;
using FluentValidation;
using KooliProjekt.Application.Data;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Payments
{
    public class SavePaymentCommandValidator : AbstractValidator<SavePaymentCommand>
    {
        public static readonly string[] AllowedPaymentMethods = { "Card", "Bank transfer", "Cash" };

        public SavePaymentCommandValidator(ApplicationDbContext dbContext)
        {
            // Id = 0 -> uus kirje, Id > 0 -> olemasoleva muutmine
            RuleFor(x => x.Id)
                .Cascade(CascadeMode.Stop)
                .GreaterThanOrEqualTo(0).WithMessage("Id cannot be negative")
                .MustAsync((id, ct) => dbContext.Payments.AnyAsync(e => e.Id == id, ct))
                .When(x => x.Id > 0, ApplyConditionTo.CurrentValidator)
                .WithMessage(x => "Cannot find payment with ID " + x.Id);

            RuleFor(x => x.OrderId)
                .Cascade(CascadeMode.Stop)
                .GreaterThan(0).WithMessage("Order is required")
                .MustAsync((orderId, ct) => dbContext.Orders.AnyAsync(o => o.Id == orderId, ct))
                .WithMessage(x => "Cannot find order with ID " + x.OrderId)
                .MustAsync(async (command, orderId, ct) =>
                    !await dbContext.Payments.AnyAsync(p => p.OrderId == orderId && p.Id != command.Id, ct))
                .WithMessage("This order already has a payment");

            RuleFor(x => x.Amount)
                .InclusiveBetween(0.01m, 999999999m).WithMessage("Amount must be between 0.01 and 999999999")
                .PrecisionScale(11, 2, true).WithMessage("Amount can have at most 2 decimal places");

            RuleFor(x => x.PaymentDate)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Payment date is required")
                .LessThanOrEqualTo(x => DateTime.Now).WithMessage("Payment date cannot be in the future")
                .MustAsync(async (command, paymentDate, ct) =>
                {
                    var orderDate = await dbContext.Orders
                        .Where(o => o.Id == command.OrderId)
                        .Select(o => (DateTime?)o.OrderDate)
                        .FirstOrDefaultAsync(ct);

                    // Kui tellimust pole, annab OrderId reegel juba vea
                    return orderDate == null || paymentDate.Date >= orderDate.Value.Date;
                })
                .WithMessage("Payment date cannot be earlier than the order date");

            RuleFor(x => x.PaymentMethod)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Payment method is required")
                .Must(method => AllowedPaymentMethods.Contains(method))
                .WithMessage("Payment method must be one of: " + string.Join(", ", AllowedPaymentMethods));
        }
    }
}
