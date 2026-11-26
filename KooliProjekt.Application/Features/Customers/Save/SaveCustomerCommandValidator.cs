using FluentValidation;

namespace KooliProjekt.Application.Features.Customers
{
    public class SaveCustomerCommandValidator : AbstractValidator<SaveCustomerCommand>
    {
        public SaveCustomerCommandValidator()
        {
            RuleFor(command => command.Id)
                .GreaterThanOrEqualTo(0).WithMessage("Id cannot be negative");

            RuleFor(command => command.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(100).WithMessage("Name cannot be longer than 100 characters");

            RuleFor(command => command.Email)
                .NotEmpty().WithMessage("Email is required")
                .MaximumLength(100).WithMessage("Email cannot be longer than 100 characters")
                .EmailAddress().WithMessage("Email is not a valid email address");

            RuleFor(command => command.PhoneNumber)
                .NotEmpty().WithMessage("Phone number is required")
                .MaximumLength(20).WithMessage("Phone number cannot be longer than 20 characters")
                .Matches(@"^\+?[0-9 ]+$").WithMessage("Phone number can contain only digits, spaces and a leading +");

            RuleFor(command => command.Password)
                .NotEmpty().When(command => command.Id == 0)
                .WithMessage("Password is required for a new customer");

            RuleFor(command => command.Password)
                .MinimumLength(8).When(command => !string.IsNullOrEmpty(command.Password))
                .WithMessage("Password must be at least 8 characters long");
        }
    }
}
