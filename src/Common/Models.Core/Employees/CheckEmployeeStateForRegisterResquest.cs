namespace Models.Core.Employees;

public class CheckEmployeeStateForRegisterResquest
{
    public string EmployeeEmail { get; set; } = string.Empty;

    public Guid ClinicId{ get; set; } = Guid.Empty;
}
