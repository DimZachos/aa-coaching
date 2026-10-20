using Appointments.Console.Helpers;
using Appointments.Console.Models;
using Appointments.Console.Services;
using FluentValidation;

var customers = new List<Customer>();
var appointments = new List<Appointment>();

var customerValidator = new CustomerValidatorHelper();
var customerService = new CustomerService(customerValidator, customers);
var appointmentService = new AppointmentService(appointments);

bool running = true;
while (running)
{
    ConsoleHelper.PrintHeader("APPOINTMENTS SYSTEM");
    
    int choice = ConsoleHelper.ShowMenu("Main Menu", "Manage Customers", "Manage Appointments", "View All", "Exit");

    switch (choice)
    {
        case 1: ManageCustomers(); break;
        case 2: ManageAppointments(); break;
        case 3: ViewAll(); break;
        case 4: running = false; break;
    }
}

void DisplayTable<T>(IEnumerable<T> items, string title, string[] headers, Func<T, string[]> rowMapper)
{
    ConsoleHelper.PrintHeader(title);
    if (!items.Any())
    {
        ConsoleHelper.PrintError("No items.");
        ConsoleHelper.Pause();
        return;
    }
    ConsoleHelper.PrintTable(headers, items.Select(rowMapper));
    ConsoleHelper.Pause();
}

void ManageCustomers()
{
    bool inMenu = true;
    while (inMenu)
    {
        ConsoleHelper.PrintHeader("CUSTOMERS");
        int choice = ConsoleHelper.ShowMenu("Customer Menu", "Add", "View All", "Update", "Delete", "Back");

        switch (choice)
        {
            case 1: AddCustomer(); break;
            case 2: ViewAllCustomers(); break;
            case 3: UpdateCustomer(); break;
            case 4: DeleteCustomer(); break;
            case 5: inMenu = false; break;
        }
    }
}

void AddCustomer()
{
    try
    {
        int id = customerService.ReadCustomers().Count + 1;
        string name = ConsoleHelper.GetInput("Name: ");
        string email = ConsoleHelper.GetInput("Email: ");
        string phone = ConsoleHelper.GetInput("Phone (+XXXXXXXXXX): ");

        customerService.Add(new Customer { Id = id, Name = name, Email = email, PhoneNumber = phone });
    }
    catch (ValidationException ex)
    {
        foreach (var error in ex.Errors)
            ConsoleHelper.PrintError(error.ErrorMessage);
    }
    ConsoleHelper.Pause();
}

void ViewAllCustomers()
{
    DisplayTable(customerService.ReadCustomers(), "CUSTOMERS", new[] { "ID", "Name", "Email", "Phone" }, 
        c => new[] { c.Id.ToString(), c.Name, c.Email, c.PhoneNumber });
}

void UpdateCustomer()
{
    ViewAllCustomers();
    try
    {
        int id = ConsoleHelper.GetIntInput("Customer ID: ");
        var customer = customerService.ReadCustomers().FirstOrDefault(c => c.Id == id);
        if (customer == null)
        {
            ConsoleHelper.PrintError("Not found.");
            ConsoleHelper.Pause();
            return;
        }

        string name = ConsoleHelper.GetInput("New name (or press enter): ");
        if (!string.IsNullOrEmpty(name)) customer.Name = name;

        string email = ConsoleHelper.GetInput("New email (or press enter): ");
        if (!string.IsNullOrEmpty(email)) customer.Email = email;

        customerService.Update(customer);
    }
    catch (ValidationException ex)
    {
        foreach (var error in ex.Errors)
            ConsoleHelper.PrintError(error.ErrorMessage);
    }
    ConsoleHelper.Pause();
}

void DeleteCustomer()
{
    ViewAllCustomers();
    int id = ConsoleHelper.GetIntInput("Customer ID to delete: ");
    var customer = customerService.ReadCustomers().FirstOrDefault(c => c.Id == id);
    if (customer != null)
    {
        customerService.Delete(customer);
    }
    else
        ConsoleHelper.PrintError("Not found.");
    ConsoleHelper.Pause();
}

void ManageAppointments()
{
    bool inMenu = true;
    while (inMenu)
    {
        ConsoleHelper.PrintHeader("APPOINTMENTS");
        int choice = ConsoleHelper.ShowMenu("Appointment Menu", "Schedule", "View All", "Cancel", "Back");

        switch (choice)
        {
            case 1: ScheduleAppointment(); break;
            case 2: ViewAllAppointments(); break;
            case 3: inMenu = false; break;
        }
    }
}

void ScheduleAppointment()
{
    try
    {
        ViewAllCustomers();
        int customerId = ConsoleHelper.GetIntInput("Customer ID: ");
        
        if (!customerService.ReadCustomers().Any(c => c.Id == customerId))
        {
            ConsoleHelper.PrintError("Customer not found.");
            ConsoleHelper.Pause();
            return;
        }

        System.Console.WriteLine("1. Massage  2. Personal Training");
        int service = ConsoleHelper.GetIntInput("Service: ");
        var serviceType = service == 1 ? ServiceType.Massage : ServiceType.PersonalTraining;

        System.Console.WriteLine("Enter date/time (yyyy-MM-dd HH:mm):");
        string dateStr = ConsoleHelper.GetInput("Date/Time: ");
        
        if (!System.DateTime.TryParse(dateStr, out var appointmentTime))
        {
            ConsoleHelper.PrintError("Invalid date format.");
            ConsoleHelper.Pause();
            return;
        }

        int id = appointmentService.ReadAppointments().Count + 1;
        string notes = ConsoleHelper.GetInput("Notes (optional): ");

        appointmentService.Add(new Appointment
        {
            Id = id,
            CustomerId = customerId,
            ServiceType = serviceType,
            Time = appointmentTime,
            Notes = notes
        });
    }
    catch (Exception ex)
    {
        ConsoleHelper.PrintError(ex.Message);
    }
    ConsoleHelper.Pause();
}

void ViewAllAppointments()
{
    var allAppointments = appointmentService.ReadAppointments().OrderBy(a => a.Time).ToList();
    DisplayTable(allAppointments, "APPOINTMENTS", new[] { "ID", "Customer", "Service", "Date/Time", "Notes" }, 
        a => {
            var cust = customerService.ReadCustomers().FirstOrDefault(c => c.Id == a.CustomerId);
            return new[] { a.Id.ToString(), cust?.Name ?? "Unknown", a.ServiceType.ToString(), a.Time.ToString("yyyy-MM-dd HH:mm"), a.Notes };
        });
}

void ViewAll()
{
    ConsoleHelper.PrintHeader("SYSTEM DATA");
    
    var allCustomers = customerService.ReadCustomers();
    var allAppointments = appointmentService.ReadAppointments();

    if (allCustomers.Count > 0)
    {
        System.Console.WriteLine("\nCustomers:");
        var custHeaders = new[] { "ID", "Name", "Email", "Phone" };
        var custRows = allCustomers.Select(c => new[] { c.Id.ToString(), c.Name, c.Email, c.PhoneNumber }).ToArray();
        ConsoleHelper.PrintTable(custHeaders, custRows);
    }

    if (allAppointments.Count > 0)
    {
        System.Console.WriteLine("\nAppointments:");
        var apptHeaders = new[] { "ID", "Customer", "Service", "Date/Time" };
        var apptRows = allAppointments.OrderBy(a => a.Time).Select(a =>
        {
            var cust = allCustomers.FirstOrDefault(c => c.Id == a.CustomerId);
            return new[] { a.Id.ToString(), cust?.Name ?? "Unknown", a.ServiceType.ToString(), a.Time.ToString("yyyy-MM-dd HH:mm") };
        }).ToArray();
        ConsoleHelper.PrintTable(apptHeaders, apptRows);
    }
    ConsoleHelper.Pause();
}
