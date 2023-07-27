namespace Models.Core.Employees;

public class EmployeeSignUpRequest
{
    public string Name { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    //Si esta vacio, servidor genera una
    public string Pass { get; set; } = string.Empty;

    public Guid CompanyId{ get; set; } = Guid.Empty;

    public Guid ClinicId{ get; set; } = Guid.Empty;

    public EmployeeRol Rol { get; set; } = EmployeeRol.CLINIC_OFFICER;
}
