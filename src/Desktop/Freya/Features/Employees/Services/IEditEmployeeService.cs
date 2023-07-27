namespace Freya.Features.Employees.Services;

public interface IEditEmployeeService
{
    Task<bool> SaveEmployeeData(EmployeeUpdateRequest employeeUpdate);
}
