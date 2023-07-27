namespace FreyaApi.Infrastructure.Mappers;

public static class EmployeeMapper
{
    public static EmployeeResponse ConverToResponse(EmployeeTable employeeTable)
    {
        return new EmployeeResponse()
        {
            Name = employeeTable.Name,
            Email = employeeTable.Email,
            LastName = employeeTable.LastName,
            Id = employeeTable.Id,
            ClinicsId = employeeTable.Clinics.Select(clinic => clinic.Id),
            CompanyId = employeeTable.CompanyId,
            Rol = employeeTable.Rol,
        };
    }
}
