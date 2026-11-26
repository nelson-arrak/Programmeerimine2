using FluentValidation;

namespace KooliProjekt.Application.Features.Addresses
{
    public class SaveAddressCommandValidator : AbstractValidator<SaveAddressCommand>
    {
        public SaveAddressCommandValidator()
        {
            RuleFor(command => command.Id)
                .GreaterThanOrEqualTo(0).WithMessage("Id cannot be negative");

            RuleFor(command => command.Street)
                .NotEmpty().WithMessage("Street is required")
                .MaximumLength(100).WithMessage("Street cannot be longer than 100 characters");

            RuleFor(command => command.City)
                .NotEmpty().WithMessage("City is required")
                .MaximumLength(100).WithMessage("City cannot be longer than 100 characters");

            RuleFor(command => command.State)
                .NotEmpty().WithMessage("State is required")
                .MaximumLength(100).WithMessage("State cannot be longer than 100 characters");

            RuleFor(command => command.ZipCode)
                .NotEmpty().WithMessage("Zip code is required")
                .MaximumLength(20).WithMessage("Zip code cannot be longer than 20 characters");

            RuleFor(command => command.Country)
                .NotEmpty().WithMessage("Country is required")
                .MaximumLength(100).WithMessage("Country cannot be longer than 100 characters");

            RuleFor(command => command.CustomerId)
                .GreaterThan(0).When(command => command.CustomerId.HasValue)
                .WithMessage("Customer ID must be greater than 0");
        }
    }
}
