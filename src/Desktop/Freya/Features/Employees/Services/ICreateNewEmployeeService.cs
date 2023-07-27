namespace Freya.Features.Employees.Services;

public interface ICreateNewEmployeeService
{
    Task<bool> CreateEmployee(Employee employee, Guid clinicId);
}
