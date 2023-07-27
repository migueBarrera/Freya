﻿namespace Models.Core.Employees;

public class EmployeeSignInResponse
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Token { get; set; } = string.Empty;

    public string RefreshToken { get; set; } = string.Empty;

    public Guid CompanyId { get; set; } = Guid.Empty;

    public EmployeeRol Rol { get; set; } = EmployeeRol.CLINIC_OFFICER;

    public IEnumerable<ClinicResponse> Clinics { get; set; } = Enumerable.Empty<ClinicResponse>();
}
