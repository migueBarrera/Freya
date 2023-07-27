using Models.Core.Common;

namespace Freya.Features.Employees.Services;

public interface IEmployeeService
{
    Task<PagedModel<EmployeeResponse>> GetEmployeesAsync(PaginationFilter PaginationFilter);

    Task<bool> DeleteEmployeeFromClinic(Guid employeeId, Guid clinicId);
}
