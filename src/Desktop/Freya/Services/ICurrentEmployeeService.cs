namespace Freya.Services;

public interface ICurrentEmployeeService
{
    public Employee? Employee { get; }

    Task SetEmployee(Employee? employee);
}
