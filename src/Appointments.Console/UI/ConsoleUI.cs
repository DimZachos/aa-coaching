using Appointments.Console.Helpers;

namespace Appointments.Console.UI;

public class ConsoleUi(CustomerUi customerUi, AppointmentUi appointmentUi)
{
    private const string Title = "APPOINTMENT SYSTEM";
    private bool _running = true;
    public void Run()
    {
        while (_running)
        {
            System.Console.WriteLine(Title.PadLeft(50));
            System.Console.WriteLine(new string('=', 90));

            int choice = ShowMenu("Main Menu", new[]
            {
                "Manage Customers",
                "Manage Appointments",
                "View All",
                "Exit"
            });

            switch (choice)
            {
                case 1:
                    customerUi.ManageCustomers();
                    break;
                case 2:
                    appointmentUi.ManageAppointments();
                    break;
                case 3:
                    ShowAll();
                    break;
                case 4:
                    _running = false;
                    break;
            }
        }
    }

    private void ShowAll()
    {
        customerUi.DisplayCustomers();
        appointmentUi.DisplayAppointments();
        ConsoleHelper.Pause();
    }

    public static int ShowMenu(string title, string[] options)
    {
        int choice = ConsoleHelper.ShowMenu(title, options);
        if (choice >= 1 && choice <= options.Length)
            return choice;

        ConsoleHelper.PrintError("Invalid choice.");
        return ShowMenu(title, options);
    }
}
