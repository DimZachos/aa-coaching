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

    public void Update(Appointment appointment)
    {
        var existingAppointment = _appointments.FirstOrDefault(a => a.Id == appointment.Id);
        if (existingAppointment == null)
            throw new KeyNotFoundException($"Appointment with Id {appointment.Id} not found.");

        existingAppointment.CustomerId = appointment.CustomerId;
        existingAppointment.ServiceTypeEnum = appointment.ServiceTypeEnum;
        existingAppointment.Time = appointment.Time;
        existingAppointment.Notes = appointment.Notes;
    }
    
    public void Delete(Appointment appointment)
    {
        _appointments.Remove(appointment);
    }
    
}