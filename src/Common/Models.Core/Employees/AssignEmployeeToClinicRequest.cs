namespace Models.Core.Employees;

public class AssignEmployeeToClinicRequest
{
    public Guid ClinicId { get; set; } = Guid.Empty;
    public string EmployeeEmail { get; set; } = string.Empty;
}
