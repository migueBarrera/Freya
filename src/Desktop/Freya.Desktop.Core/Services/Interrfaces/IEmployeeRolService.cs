namespace Freya.Desktop.Core.Services.Interrfaces;

public interface IEmployeeRolService
{
    bool IsCompanyAdmin(EmployeeRol employeeRol);

    bool IsClinicManagerOrMinor(EmployeeRol employeeRol);

    bool IsClinicManagerOrHigher(EmployeeRol employeeRol);
}
