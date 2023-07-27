namespace Freya.Services;

internal class CurrentEmployeeService : ICurrentEmployeeService
{
    private Employee? _employee;

    public CurrentEmployeeService()
    {

    }

    public Employee? Employee => _employee;

    public Task SetEmployee(Employee? employee)
    {
        _employee = employee;
        return Task.CompletedTask;
    }
}
