using Appointments.Console;
using Appointments.Console.Models;
using Appointments.Console.Services;
using Appointments.Console.UI;

var customers = new List<Customer>();
var appointments = new List<Appointment>();

var customerValidator = new CustomerValidatorHelper();
var customerService = new CustomerService(customerValidator, customers);
var appointmentService = new AppointmentService(appointments);
var customerUi = new CustomerUi(customerService);
var appointmentUi = new AppointmentUi(appointmentService, customerService);
var consoleUi = new ConsoleUi(customerUi, appointmentUi);

consoleUi.Run();
