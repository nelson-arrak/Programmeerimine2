using FluentValidation;
using KooliProjekt.Application.Data;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Customers
{
    public class DeleteCustomerCommandValidator : AbstractValidator<DeleteCustomerCommand>
    {
        public DeleteCustomerCommandValidator(ApplicationDbContext dbContext)
        {
            RuleFor(x => x.Id)
                .Cascade(CascadeMode.Stop)
                .GreaterThan(0).WithMessage("Id must be greater than 0")
                .MustAsync((id, ct) => dbContext.Customers.AnyAsync(e => e.Id == id, ct))
                .WithMessage(x => "Cannot find customer with ID " + x.Id)
                .MustAsync(async (id, ct) => !await dbContext.Orders.AnyAsync(o => o.Customer.Id == id, ct))
                .WithMessage("Cannot delete customer because the customer has orders");
        }
    }
}
