using Appointments.Console.Interfaces;
using Appointments.Console.Models;
using FluentValidation;

namespace Appointments.Console.Services;

public class CustomerService : ICustomerService
{
    private readonly List<Customer> _customers;
    private readonly AbstractValidator<Customer> _customersValidator;

    public CustomerService(AbstractValidator<Customer> customerValidator, List<Customer> customers)
    {
        _customersValidator = customerValidator;
        _customers = customers;
    }
    
    public void Add(Customer customer)
    {
        var customerValidation = _customersValidator.Validate(customer);
        if (!customerValidation.IsValid)
            throw new ValidationException(customerValidation.Errors);
        
        _customers.Add(customer);
    }

    public IReadOnlyList<Customer> ReadCustomers() => _customers;

    public void Update(Customer customer)
    {
        var customerValidation = _customersValidator.Validate(customer);
        if (!customerValidation.IsValid)
            throw new ValidationException(customerValidation.Errors);

        var existingCustomer = _customers.FirstOrDefault(c => c.Id == customer.Id);
        if (existingCustomer == null)
             throw new KeyNotFoundException($"Customer with Id {customer.Id} not found.");

        existingCustomer.Name = customer.Name;
        existingCustomer.Email = customer.Email;
        existingCustomer.PhoneNumber = customer.PhoneNumber;
    }
    
    public void Delete(Customer customer)
    {
        _customers.Remove(customer);
    }
}