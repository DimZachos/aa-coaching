using Appointments.Console.Interfaces;
using Appointments.Console.Models;

namespace Appointments.Console.Services;

public class AppointmentService : IAppointmentService
{
    private readonly List<Appointment> _appointments;

    public AppointmentService(List<Appointment> appointments)
    {
        _appointments = appointments;
    }
    
    public void Add(Appointment appointment)
    {
        _appointments.Add(appointment);
    }

    public IReadOnlyList<Appointment> ReadAppointments() => _appointments;
    
    public void Delete(Appointment appointment)
    {
        _appointments.Remove(appointment);
    }
    
}