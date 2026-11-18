using Appointments.Console.Helpers;
using Appointments.Console.Interfaces;
using Appointments.Console.Models;
using FluentValidation;

namespace Appointments.Console.UI;

public class CustomerUi(ICustomerService customerService)
{
    private const string CustomersTitle = "CUSTOMERS";
    
    public void ManageCustomers()
    {
        System.Console.WriteLine(CustomersTitle.PadLeft(50));
        int choice = ConsoleUi.ShowMenu("Customer Menu", new[]
        {
            "Add",
            "View All",
            "Update",
            "Delete",
            "Back"
        });

        switch (choice)
        {
            case 1: AddCustomer();
                    break;
            case 2: ViewAllCustomers();
                    break;
            case 3: UpdateCustomer();
                    break;
            case 4: DeleteCustomer();
                    break;
            case 5:
                    break;
        }
    }

    public void DisplayCustomers()
    {
        var customers = customerService.ReadCustomers();
        if (customers.Count == 0)
        {
            ConsoleHelper.PrintError("No items.");
            return;
        }
        
        DisplayCustomerList(customers);
    }

    private static void DisplayCustomerList(IReadOnlyList<Customer> customers)
    {
        System.Console.WriteLine(CustomersTitle.PadLeft(50));
        var customerLines = new List<string> { "ID | Name | Email | Phone" };
        foreach (var customer in customers)
        {
            customerLines.Add($"{customer.Id} | {customer.Name} | {customer.Email} | {customer.PhoneNumber}");
        }

        ConsoleHelper.Print(string.Join(Environment.NewLine, customerLines));
    }

    private void AddCustomer()
    {
        try
        {
            var customers = customerService.ReadCustomers();
            int id = customers.Count == 0 ? 1 : customers.Max(c => c.Id) + 1;
            string name = ConsoleHelper.GetInput("Name: ");
            string email = ConsoleHelper.GetInput("Email: ");
            string phone = ConsoleHelper.GetInput("Phone (1234567890): ");

            customerService.Add(new Customer
            {
                Id = id,
                Name = name,
                Email = email,
                PhoneNumber = phone
            });
        }
        catch (ValidationException ex)
        {
            foreach (var error in ex.Errors)
                ConsoleHelper.PrintError(error.ErrorMessage);
        }

        ConsoleHelper.Pause();
    }

    private void ViewAllCustomers()
    {
        System.Console.WriteLine(CustomersTitle.PadLeft(50));
        var customers = customerService.ReadCustomers();
        if (customers.Count == 0)
        {
            ConsoleHelper.PrintError("No items.");
            ConsoleHelper.Pause();
            return;
        }

        DisplayCustomerList(customers);
        ConsoleHelper.Pause();
    }

    private void UpdateCustomer()
    {
        var customers = customerService.ReadCustomers();
        if (customers.Count == 0)
        {
            ConsoleHelper.PrintError("No items.");
            ConsoleHelper.Pause();
            return;
        }

        System.Console.WriteLine(CustomersTitle.PadLeft(50));
        DisplayCustomerList(customers);
        string customerIdInput = ConsoleHelper.GetInput("Customer ID: ");
        if (!int.TryParse(customerIdInput, out int customerId))
        {
            ConsoleHelper.PrintError("Invalid number.");
            ConsoleHelper.Pause();
            return;
        }
        
        try
        {
            var customer = customers.FirstOrDefault(c => c.Id == customerId);
            if (customer == null)
            {
                ConsoleHelper.PrintError("Not found.");
                ConsoleHelper.Pause();
                return;
            }

            string name = ConsoleHelper.GetInput($"New name ({customer.Name}): ");
            string email = ConsoleHelper.GetInput($"New email ({customer.Email}): ");
            string phone = ConsoleHelper.GetInput($"New phone ({customer.PhoneNumber}): ");

            customerService.Update(new Customer
            {
                Id = customer.Id,
                Name = string.IsNullOrEmpty(name) ? customer.Name : name,
                Email = string.IsNullOrEmpty(email) ? customer.Email : email,
                PhoneNumber = string.IsNullOrEmpty(phone) ? customer.PhoneNumber : phone
            });
        }
        catch (ValidationException ex)
        {
            foreach (var error in ex.Errors)
                ConsoleHelper.PrintError(error.ErrorMessage);
        }

        ConsoleHelper.Pause();
    }

    private void DeleteCustomer()
    {
        var customers = customerService.ReadCustomers();
        if (customers.Count == 0)
        {
            ConsoleHelper.PrintError("No items.");
            ConsoleHelper.Pause();
            return;
        }

        System.Console.WriteLine(CustomersTitle.PadLeft(50));
        DisplayCustomerList(customers);
        string idInput = ConsoleHelper.GetInput("Which customer do you want to delete? ");
        
        if (!int.TryParse(idInput, out int id))
        {
            ConsoleHelper.PrintError("Invalid number.");
            ConsoleHelper.Pause();
            return;
        }

        var customer = customers.FirstOrDefault(c => c.Id == id);
        
        if (customer != null)
            customerService.Delete(customer);
        else
            ConsoleHelper.PrintError("Not found.");

        ConsoleHelper.Pause();
    }
}
