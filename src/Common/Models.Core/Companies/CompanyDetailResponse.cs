using Models.Core.Common;

namespace Models.Core.Companies;

public class CompanyDetailResponse
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public PagedModel<EmployeeResponse> Employees { get; set; } = PagedModel<EmployeeResponse>.Empty();

    public PagedModel<ClinicResponse> Clinics { get; set; } = PagedModel<ClinicResponse>.Empty();

    public IEnumerable<PaymentsPlansForCompanyResponse> PaymentsPlansForCompanyResponses { get; set; } = Enumerable.Empty<PaymentsPlansForCompanyResponse>();

    public DateTime Created { get; set; } = DateTime.UtcNow;
}
