using Appointments.Console.Enums;

namespace Appointments.Console.Models;

public class Appointment
{
    public int Id { get; init; }
    public int CustomerId { get; set; }
    public ServiceTypeEnum ServiceTypeEnum { get; set; }
    public DateTime Time { get; set; }
    public string Notes { get; set; } = string.Empty;
}