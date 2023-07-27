namespace Models.Core.Employees;

public class EmployeeSignUpResponse
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Token { get; set; } = string.Empty;

    public string RefrehToken { get; set; } = string.Empty;

    public EmployeeRol Rol { get; set; } = EmployeeRol.CLINIC_OFFICER;

    public IEnumerable<ClinicResponse> Clinics { get; set; } = Enumerable.Empty<ClinicResponse>();
}
