using Appointments.Console.Models;
using FluentValidation;

namespace Appointments.Console.Helpers;

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
            .Matches(@"^\+\d{10,}$")
            .WithMessage("Phone must be in format (+XXXXXXXXXX)");
    }
}