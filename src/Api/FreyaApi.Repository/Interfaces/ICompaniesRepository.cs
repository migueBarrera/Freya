using Models.Core.Companies;

namespace FreyaApi.Repository.Interfaces;

public interface ICompaniesRepository
{
    Task<bool> CreateCompany(CompanyTable company);

    IQueryable<Company> GetCompanies();

    Task<CompanyTable?> GetCompanyById(Guid id);

    IQueryable<EmployeeTable> GetEmployeeForCompany(Guid id, string? searchArgument = null);

    IQueryable<ClinicTable> GetClinicsForCompany(Guid id, string? searchArgument = null);

    Task<bool> Update(UpdateCommpanyRequest request);

    Task<bool> DeleteCompany(Guid companyId);
}
