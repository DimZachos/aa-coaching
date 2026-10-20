using Appointments.Console.Models;

namespace Appointments.Console.Interfaces;

public interface ICustomerService
{
    void Add(Customer customer);
    IReadOnlyList<Customer> ReadCustomers();
    void Update(Customer customer);
    void Delete(Customer customer);
}