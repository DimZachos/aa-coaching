using Appointments.Console.Models;
using FluentValidation;

namespace Appointments.Console;

public class CustomerValidatorHelper : AbstractValidator<Customer>
{
    public CustomerValidatorHelper()
    {
        RuleFor(c => c.Id)
            .NotEmpty()
            .NotNull()
            .WithMessage("Id is null or empty");
        
        RuleFor(c => c.Name)
            .NotEmpty()
            .WithMessage("Name is required");
        
        RuleFor(c => c.Email)
            .NotEmpty()
            .EmailAddress()
            .WithMessage("Email must be valid");
        
        RuleFor(c => c.PhoneNumber)
            .NotEmpty()
            .Must(phoneNumber => phoneNumber != null && phoneNumber.All(char.IsDigit))
            .Length(10)
            .WithMessage("Phone must be in format (1234567890)");
    }
}