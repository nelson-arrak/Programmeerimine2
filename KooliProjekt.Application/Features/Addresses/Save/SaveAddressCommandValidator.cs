using System;
using System.Linq;
using FluentValidation;
using KooliProjekt.Application.Data;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Addresses
{
    public class SaveAddressCommandValidator : AbstractValidator<SaveAddressCommand>
    {

        public SaveAddressCommandValidator(ApplicationDbContext dbContext)
        {
            // Id = 0 -> uus kirje, Id > 0 -> olemasoleva muutmine
            RuleFor(x => x.Id)
                .Cascade(CascadeMode.Stop)
                .GreaterThanOrEqualTo(0).WithMessage("Id cannot be negative")
                .MustAsync((id, ct) => dbContext.Addresses.AnyAsync(e => e.Id == id, ct))
                .When(x => x.Id > 0, ApplyConditionTo.CurrentValidator)
                .WithMessage(x => "Cannot find address with ID " + x.Id);

            RuleFor(x => x.Street)
                .NotEmpty().WithMessage("Street is required")
                .MaximumLength(100).WithMessage("Street cannot be longer than 100 characters");

            RuleFor(x => x.City)
                .NotEmpty().WithMessage("City is required")
                .MaximumLength(100).WithMessage("City cannot be longer than 100 characters");

            RuleFor(x => x.State)
                .NotEmpty().WithMessage("State is required")
                .MaximumLength(100).WithMessage("State cannot be longer than 100 characters");

            RuleFor(x => x.ZipCode)
                .NotEmpty().WithMessage("Zip code is required")
                .MaximumLength(20).WithMessage("Zip code cannot be longer than 20 characters")
                .Matches(@"^[A-Za-z0-9 \-]+$").WithMessage("Zip code can contain only letters, digits, spaces and hyphens");

            RuleFor(x => x.Country)
                .NotEmpty().WithMessage("Country is required")
                .MaximumLength(100).WithMessage("Country cannot be longer than 100 characters");

            RuleFor(x => x.CustomerId)
                .Cascade(CascadeMode.Stop)
                .GreaterThan(0).WithMessage("Customer ID must be greater than 0")
                .MustAsync((customerId, ct) => dbContext.Customers.AnyAsync(c => c.Id == customerId, ct))
                .WithMessage(x => "Cannot find customer with ID " + x.CustomerId)
                .When(x => x.CustomerId.HasValue);
        }
    }
}
