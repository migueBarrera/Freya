namespace FreyaApi.Repository.Services;

public class EmployeeRepository : BaseRepository, IEmployeeRepository
{
    public EmployeeRepository(DatabaseContext databaseContext)
         : base(databaseContext)
    {
    }

    public async Task<bool> ChangePassword(Guid userID, string newPassHashed)
    {
        var employee = await context.Employee?
            .FirstOrDefaultAsync(a => a.Id == userID)!;
        if (employee == null)
        {
            return false;
        }

        employee.Pass = newPassHashed;

        await context.SaveChangesAsync();
        return true;
    }

    public async Task<EmployeeTable?> CreateEmployee(EmployeeSignUpRequest request, string pass, Guid clinicId)
    {
        var employee = new EmployeeTable()
        {
            Name = request.Name,
            Pass = pass,
            Email = request.Email,
            LastName = request.LastName,
            Rol = request.Rol,
            CompanyId = request.CompanyId,
        };

        bool needAsignToClinic = clinicId != Guid.Empty;
        if (needAsignToClinic)
        {
            var clinic = await context
                .Clinic?
                .FirstOrDefaultAsync(c => c.Id == request.ClinicId)!;
            if (clinic != null)
            {
                employee.Clinics.Add(clinic);
            }
        }

        context.Employee?.Add(employee);

        await context.SaveChangesAsync();
        return employee;
    }

    public bool EmployeeExists(Guid id)
    {
        return context.Employee?.Any(e => e.Id == id) ?? false;
    }

    public bool EmployeeExists(string email)
    {
        return context.Employee?.Any(e => e.Email == email) ?? false;
    }

    public IQueryable<EmployeeTable> GetAllEmployeeByCompanyId(Guid id, string? searchArgument = null)
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

    public Task<EmployeeTable?> GetEmployee(Guid id)
    {
        return context.Employee?
            .AsNoTracking()
        .FirstOrDefaultAsync(a => a.Id == id)!;
    }

    public Task<EmployeeTable?> GetEmployee(string email)
    {
        return context.Employee?
           .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Email == email)!;
    }

    public async Task<EmployeeTable?> GetEmployeeWithClinicData(string email)
    {
        return await context.Employee?
            .Where(a => a.Email == email)
            .Include(c => c.Clinics)
            .AsNoTracking()
            .FirstOrDefaultAsync()!;
    }

    public async Task<bool> ResetPassword(string email, string hashedPass)
    {
        EmployeeTable? employee = context.Employee?
            .FirstOrDefault(a => a.Email == email);

        if (employee == null)
        {
            return false;
        }

        employee.Pass = hashedPass;
        await context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> Update(EmployeeUpdateRequest request)
    {
        EmployeeTable? employee = context.Employee?
            .FirstOrDefault(a => a.Id == request.Id);

        if (employee == null)
        {
            return false;
        }

        employee.Name = request.Name;
        employee.LastName = request.LastName;

        await context.SaveChangesAsync();
        return true;
    }
}
