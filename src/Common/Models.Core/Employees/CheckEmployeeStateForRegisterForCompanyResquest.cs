namespace Models.Core.Employees;

public class CheckEmployeeStateForRegisterForCompanyResquest
{
    public string EmployeeEmail { get; set; } = string.Empty;

    public Guid ConpanyId { get; set; } = Guid.Empty;
}
