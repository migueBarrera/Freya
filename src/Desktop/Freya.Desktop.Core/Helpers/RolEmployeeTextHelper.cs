namespace Freya.Desktop.Core.Helpers;

public static class RolEmployeeTextHelper
{
    public static string GetRolResourceKeyByRol(EmployeeRol employeeRol)
    {
        return employeeRol switch
        {
            EmployeeRol.COMPANY_OWNER => "employee_rol_company_owner",
            EmployeeRol.COMPANY_MANAGER => "employee_rol_company_manager",
            EmployeeRol.CLINIC_MANAGER => "employee_rol_clinic_manager",
            EmployeeRol.CLINIC_OFFICER => "employee_rol_clinic_office",
            _ => "employee_rol_clinic_office",
        };
    }
}
