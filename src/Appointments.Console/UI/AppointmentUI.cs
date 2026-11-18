using Appointments.Console.Enums;
using Appointments.Console.Helpers;
using Appointments.Console.Interfaces;
using Appointments.Console.Models;

namespace Appointments.Console.UI;

public class AppointmentUi(IAppointmentService appointmentService, ICustomerService customerService)
{
    private const string AppointmentsTitle = "APPOINTMENTS";

    public void ManageAppointments()
    {
        System.Console.WriteLine(AppointmentsTitle.PadLeft(50));
        int choice = ConsoleUi.ShowMenu("Appointment Menu", new[]
        {
            "Create",
            "View All",
            "Update",
            "Back"
        });

        switch (choice)
        {
            case 1: CreateAppointment();
                    break;
            case 2: ViewAllAppointments();
                    break;
            case 3: UpdateAppointment();
                    break;
            case 4: 
                    break;
        }
    }

    public void DisplayAppointments()
    {
        var appointments = appointmentService.ReadAppointments();
        var customers = customerService.ReadCustomers();
        
        if (appointments.Count == 0)
        {
            ConsoleHelper.PrintError("No items.");
            return;
        }
        
        DisplayAppointmentList(appointments, customers);
    }

    private static void DisplayAppointmentList(IReadOnlyList<Appointment> appointments, IReadOnlyList<Customer> customers)
    {
        System.Console.WriteLine(AppointmentsTitle.PadLeft(50));
        var appointmentLines = new List<string> { "ID | Customer | Service | Date/Time | Notes" };
        
        foreach (var appointment in appointments.OrderBy(a => a.Time))
        {
            var customer = customers.FirstOrDefault(c => c.Id == appointment.CustomerId);
            string customerName = customer?.Name ?? "Unknown";
            appointmentLines.Add($"{appointment.Id} | {customerName} | {appointment.ServiceTypeEnum} | {appointment.Time:yyyy-MM-dd HH:mm} | {appointment.Notes}");
        }

        ConsoleHelper.Print(string.Join(Environment.NewLine, appointmentLines));
    }

    private void CreateAppointment()
    {
        var customers = customerService.ReadCustomers();
        System.Console.WriteLine(AppointmentsTitle.PadLeft(50));

        if (customers.Count == 0)
        {
            ConsoleHelper.PrintError("No items.");
            ConsoleHelper.Pause();
            return;
        }

        var customerLines = new List<string> { "ID | Name | Email | Phone" };
        foreach (var customer in customers)
        {
            customerLines.Add($"{customer.Id} | {customer.Name} | {customer.Email} | {customer.PhoneNumber}");
        }

        ConsoleHelper.Print(string.Join(Environment.NewLine, customerLines));

        string customerIdInput = ConsoleHelper.GetInput("Customer ID: ");
        if (!int.TryParse(customerIdInput, out int customerId))
        {
            ConsoleHelper.PrintError("Invalid number.");
            ConsoleHelper.Pause();
            return;
        }

        if (!customers.Any(c => c.Id == customerId))
        {
            ConsoleHelper.PrintError("Customer not found.");
            ConsoleHelper.Pause();
            return;
        }

        System.Console.WriteLine("1. Massage  2. Personal Training");
        string serviceInput = ConsoleHelper.GetInput("Service: ");
        if (!int.TryParse(serviceInput, out int service))
        {
            ConsoleHelper.PrintError("Invalid number.");
            ConsoleHelper.Pause();
            return;
        }

        if (service != 1 && service != 2)
        {
            ConsoleHelper.PrintError("Invalid service.");
            ConsoleHelper.Pause();
            return;
        }

        var serviceType = service == 1 ? ServiceTypeEnum.Massage : ServiceTypeEnum.PersonalTraining;

        System.Console.WriteLine("Enter date/time (yyyy-MM-dd HH:mm):");
        string dateStr = ConsoleHelper.GetInput("Date/Time: ");
        
        if (!DateTime.TryParse(dateStr, out var appointmentTime))
        {
            ConsoleHelper.PrintError("Invalid date format.");
            ConsoleHelper.Pause();
            return;
        }

        var appointments = appointmentService.ReadAppointments();
        int id = appointments.Count == 0 ? 1 : appointments.Max(a => a.Id) + 1;
        string notes = ConsoleHelper.GetInput("Notes (optional): ");
        appointmentService.Add(new Appointment
        {
            Id = id,
            CustomerId = customerId,
            ServiceTypeEnum = serviceType,
            Time = appointmentTime,
            Notes = notes
        });

        ConsoleHelper.Pause();
    }

    private void ViewAllAppointments()
    {
        DisplayAppointments();
        ConsoleHelper.Pause();
    }

    private void UpdateAppointment()
    {
        DisplayAppointments();
        var appointments = appointmentService.ReadAppointments();
        if (appointments.Count == 0)
        {
            ConsoleHelper.Pause();
            return;
        }

        string idInput = ConsoleHelper.GetInput("Appointment ID: ");
        if (!int.TryParse(idInput, out int id))
        {
            ConsoleHelper.PrintError("Invalid number.");
            ConsoleHelper.Pause();
            return;
        }

        var appointment = appointments.FirstOrDefault(a => a.Id == id);
        
        if (appointment == null)
        {
            ConsoleHelper.PrintError("Not found.");
            ConsoleHelper.Pause();
            return;
        }

        System.Console.WriteLine("1. Massage  2. Personal Training");
        string serviceInput = ConsoleHelper.GetInput("Service Type (1. Massage  2. Personal Training): ");
        int service = appointment.ServiceTypeEnum == ServiceTypeEnum.Massage ? 1 : 2;
        
        if (!string.IsNullOrEmpty(serviceInput) && (!int.TryParse(serviceInput, out service) || (service != 1 && service != 2)))
        {
            ConsoleHelper.PrintError("Invalid service.");
            ConsoleHelper.Pause();
            return;
        }

        string dateStr = ConsoleHelper.GetInput($"Date/Time ({appointment.Time:yyyy-MM-dd HH:mm} to keep current): ");
        DateTime appointmentTime = appointment.Time;
        
        if (!string.IsNullOrEmpty(dateStr) && !DateTime.TryParse(dateStr, out appointmentTime))
        {
            ConsoleHelper.PrintError("Invalid date format.");
            ConsoleHelper.Pause();
            return;
        }

        string notes = ConsoleHelper.GetInput("New notes: ");
        var updatedAppointment = new Appointment
        {
            Id = appointment.Id,
            CustomerId = appointment.CustomerId,
            ServiceTypeEnum = service == 1 ? ServiceTypeEnum.Massage : ServiceTypeEnum.PersonalTraining,
            Time = appointmentTime,
            Notes = string.IsNullOrEmpty(notes) ? appointment.Notes : notes
        };
        appointmentService.Update(updatedAppointment);
        ConsoleHelper.Pause();
    }

}
