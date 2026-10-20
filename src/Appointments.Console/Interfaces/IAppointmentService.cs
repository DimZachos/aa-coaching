using Appointments.Console.Models;

namespace Appointments.Console.Interfaces;

public interface IAppointmentService
{
    void Add(Appointment appointment);
    IReadOnlyList<Appointment> ReadAppointments();
    void Delete(Appointment appointment);
}