namespace Freya.Features.Employees.Services;

public interface ICheckEmployeeService
{
    Task<Result<NewEmployeeState>> CheckEmployeeState(string email, Guid clinicId);
    Task<Result<bool>> AssignEmployeeToSelectedClinic(string email, Guid clinicId);
}
