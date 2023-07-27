namespace Freya.Desktop.Core.Services;

public class EmployeeRolService : IEmployeeRolService
{
    public bool IsCompanyAdmin(EmployeeRol employeeRol)
    {
        return employeeRol == EmployeeRol.COMPANY_OWNER || employeeRol == EmployeeRol.COMPANY_MANAGER;
    }

    public bool IsClinicManagerOrMinor(EmployeeRol employeeRol)
    {
        return employeeRol == EmployeeRol.CLINIC_OFFICER || employeeRol == EmployeeRol.CLINIC_MANAGER;
    }

    public bool IsClinicManagerOrHigher(EmployeeRol employeeRol)
    {
        return employeeRol == EmployeeRol.COMPANY_MANAGER
            || employeeRol == EmployeeRol.CLINIC_MANAGER
            || employeeRol == EmployeeRol.COMPANY_OWNER;
    }
}
