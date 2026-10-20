namespace Appointments.Console.Models;

public class Appointment
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public ServiceType ServiceType { get; set; }
    public DateTime Time { get; set; }
    public string Notes { get; set; } = string.Empty;
}