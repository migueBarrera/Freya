namespace Models.Core.Employees;

public class UnassignEmployeeToClinicRequest
{
    public Guid ClinicId { get; set; } = Guid.Empty;
    public Guid EmployeeId { get; set; } = Guid.Empty;
}
