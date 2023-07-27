namespace Models.Core.Employees;

public  class EmployeeUpdateRequest
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;
}
