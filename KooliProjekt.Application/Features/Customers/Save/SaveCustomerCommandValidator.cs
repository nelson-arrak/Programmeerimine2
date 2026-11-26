using System;
using System.Linq;
using FluentValidation;
using KooliProjekt.Application.Data;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Customers
{
    public class SaveCustomerCommandValidator : AbstractValidator<SaveCustomerCommand>
    {

        public SaveCustomerCommandValidator(ApplicationDbContext dbContext)
        {
            // Id = 0 -> uus kirje, Id > 0 -> olemasoleva muutmine
            RuleFor(x => x.Id)
                .Cascade(CascadeMode.Stop)
                .GreaterThanOrEqualTo(0).WithMessage("Id cannot be negative")
                .MustAsync((id, ct) => dbContext.Customers.AnyAsync(e => e.Id == id, ct))
                .When(x => x.Id > 0, ApplyConditionTo.CurrentValidator)
                .WithMessage(x => "Cannot find customer with ID " + x.Id);

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(100).WithMessage("Name cannot be longer than 100 characters");

            RuleFor(x => x.Email)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Email is required")
                .MaximumLength(100).WithMessage("Email cannot be longer than 100 characters")
                .EmailAddress().WithMessage("Email is not a valid email address")
                .MustAsync(async (command, email, ct) =>
                    !await dbContext.Customers.AnyAsync(c => c.Email == email && c.Id != command.Id, ct))
                .WithMessage("Customer with this email already exists");

            RuleFor(x => x.PhoneNumber)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Phone number is required")
                .MaximumLength(20).WithMessage("Phone number cannot be longer than 20 characters")
                .Matches(@"^\+?[0-9 ]+$").WithMessage("Phone number can contain only digits, spaces and a leading +")
                .MustAsync(async (command, phone, ct) =>
                    !await dbContext.Customers.AnyAsync(c => c.PhoneNumber == phone && c.Id != command.Id, ct))
                .WithMessage("Customer with this phone number already exists");

            // Uue kliendi puhul on parool kohustuslik, muutmisel tühi parool = parooli ei muudeta
            RuleFor(x => x.Password)
                .NotEmpty().When(x => x.Id == 0)
                .WithMessage("Password is required for a new customer");

            RuleFor(x => x.Password)
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long")
                .MaximumLength(100).WithMessage("Password cannot be longer than 100 characters")
                .When(x => !string.IsNullOrEmpty(x.Password));
        }
    }
}
