namespace Models.Core.Employees;

public class EmployeeChangePassRequest
{
    public string CurrentPass { get; set; } = string.Empty;

    public string NewPass { get; set; } = string.Empty;
}
