using FluentValidation;
using KooliProjekt.Application.Data;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Addresses
{
    public class DeleteAddressCommandValidator : AbstractValidator<DeleteAddressCommand>
    {
        public DeleteAddressCommandValidator(ApplicationDbContext dbContext)
        {
            RuleFor(x => x.Id)
                .Cascade(CascadeMode.Stop)
                .GreaterThan(0).WithMessage("Id must be greater than 0")
                .MustAsync((id, ct) => dbContext.Addresses.AnyAsync(e => e.Id == id, ct))
                .WithMessage(x => "Cannot find address with ID " + x.Id);
        }
    }
}
