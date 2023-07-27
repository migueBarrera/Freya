using FreyaApi.Infrastructure.Mappers;
using Models.Core.Companies;

namespace FreyaApi.Repository.Services;

public class CompaniesRepository : BaseRepository, ICompaniesRepository
{
    public CompaniesRepository(DatabaseContext context)
         : base(context)
    {
        this.context = context;
    }

    public async Task<bool> CreateCompany(CompanyTable company)
    {
        context.Company?.Add(company);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteCompany(Guid companyId)
    {
        var company = context.Company?
            .FirstOrDefault(a => a.Id == companyId);
        if (company == null)
        {
            return false;
        }

        context.Remove(company);

        await context.SaveChangesAsync();
        return true;
    }

    public IQueryable<ClinicTable> GetClinicsForCompany(Guid id, string? searchArgument = null)
    {
        IQueryable<ClinicTable>? employees;
        if (string.IsNullOrWhiteSpace(searchArgument))
        {
            employees = context.Clinic?
                .OrderByDescending(c => c.Created)
                  .Where(x => x.CompanyId == id)
                  .AsNoTracking()!;
        }
        else
        {
            employees = context.Clinic?
                    .OrderByDescending(c => c.Created)
                  .Where(c => c.Name.Contains(searchArgument) || c.NIF.Contains(searchArgument) || c.Adress.Contains(searchArgument))
                  .Where(x => x.CompanyId == id)
                  .AsNoTracking()!;
        }

        return employees ?? Enumerable.Empty<ClinicTable>().AsQueryable();
    }

    public IQueryable<Company> GetCompanies()
    {
        var response = context
            .Company?
            .OrderByDescending(c => c.Created)
            .AsNoTracking()
            .Select(x => CompanyMapper.ConvertTo(x))!;

        return response ?? Enumerable.Empty<Company>().AsQueryable();
    }

    public async Task<CompanyTable?> GetCompanyById(Guid id)
    {
        var company = await context.Company?
            //.Include(c => c.Employees)
            //    .Include(c => c.Clinics)
            //        .ThenInclude(cl => cl.Employees)
             .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == id)!;

        return company;
    }

    public IQueryable<EmployeeTable> GetEmployeeForCompany(Guid id, string? searchArgument = null)
    {
        IQueryable<EmployeeTable>? employees;
        if (string.IsNullOrWhiteSpace(searchArgument))
        {
            employees = context.Employee?
                .OrderByDescending(c => c.Created)
                  .Where(x => x.CompanyId == id)
                  .AsNoTracking()!;
        }
        else
        {
            employees = context.Employee?
                    .OrderByDescending(c => c.Created)
                  .Where(c => c.Name.Contains(searchArgument) || c.LastName.Contains(searchArgument) || c.Email.Contains(searchArgument))
                  .Where(x => x.CompanyId == id)
                  .AsNoTracking()!;
        }

        return employees ?? Enumerable.Empty<EmployeeTable>().AsQueryable();
    }

    public async Task<bool> Update(UpdateCommpanyRequest request)
    {
        var company = context.Company?
            .FirstOrDefault(a => a.Id == request.Id);

        if (company == null)
        {
            return false;
        }

        company.Name = request.Name;

        var rowChanges = await context.SaveChangesAsync();
        return rowChanges > 0;
    }
}
