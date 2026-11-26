using FluentValidation;
using KooliProjekt.Application.Data;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Payments
{
    public class DeletePaymentCommandValidator : AbstractValidator<DeletePaymentCommand>
    {
        public DeletePaymentCommandValidator(ApplicationDbContext dbContext)
        {
            RuleFor(x => x.Id)
                .Cascade(CascadeMode.Stop)
                .GreaterThan(0).WithMessage("Id must be greater than 0")
                .MustAsync((id, ct) => dbContext.Payments.AnyAsync(e => e.Id == id, ct))
                .WithMessage(x => "Cannot find payment with ID " + x.Id);
        }
    }
}
