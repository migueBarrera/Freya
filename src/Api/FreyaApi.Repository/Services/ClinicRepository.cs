namespace FreyaApi.Repository.Services;

public class ClinicRepository : BaseRepository, IClinicRepository
{
    public ClinicRepository(DatabaseContext context)
         : base(context)
    {
    }

    public async Task AddClientToClinic(ClinicClient relation)
    {
        context.ClinicClients?.Add(relation);
        await context.SaveChangesAsync();
    }

    public async Task<bool> AddEmployeeToClinic(Guid clinicId, string employeeEmail)
    {
        ClinicTable? clinic = context.Clinic?
            .FirstOrDefault(a => a.Id == clinicId);

        if (clinic == null)
        {
            return false;
        }

        var employee = context.Employee?.FirstOrDefault(x => x.Email == employeeEmail);

        if (employee == null)
        {
            return false;
        }

        clinic.Employees.Add(employee);

        await context.SaveChangesAsync();

        return true;
    }

    public bool ClinicExist(Guid clinicId)
    {
        return context.Clinic?.Any(x => x.Id == clinicId) ?? false;
    }

    public async Task<bool> Create(ClinicTable clinic)
    {
        context.Clinic?.Add(clinic);
        var numberChanges = await context.SaveChangesAsync();
        bool hasChanges = numberChanges > 0;

        return hasChanges;
    }

    public async Task<bool> DeleteClinic(Guid id)
    {
        var clinic = context.Clinic?
            .FirstOrDefault(a => a.Id == id);
        if (clinic == null)
        {
            return false;
        }

        context.Remove(clinic);

        await context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<Guid>> GetAllClientsIdFromClinic(Guid id)
    {
        var clinicClients = await context
            .ClinicClients?
            .Where(x => x.ClinicId == id)
            .AsNoTracking()
            .ToListAsync()!;

        if (clinicClients == null)
        {
            return Enumerable.Empty<Guid>();
        }

        return clinicClients.Select(x => x.ClientId).ToList();
    }

    public async Task<IEnumerable<ClinicTable>> GetAllClinicFromEmployeeByRol(Guid id)
    {
        IEnumerable<ClinicTable> list = new List<ClinicTable>();

        var employee = await context.Employee?
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id)!;

        if (employee == null)
        {
            return list;
        }

        if (employee.Rol == EmployeeRol.COMPANY_MANAGER ||
            employee.Rol == EmployeeRol.COMPANY_OWNER)
        {
            list = await context.Clinic?
                .Where(c => c.CompanyId == employee.CompanyId)
                .Include(c => c.SubscriptionPayments)
                    .ThenInclude(p => p.SubscriptionPlan)
                        .ThenInclude(p => p!.SubscriptionProduct)
                .AsNoTracking()    
                .ToListAsync()! ?? new List<ClinicTable>();
        }
        else
        {
            list = await context.Clinic?
                .Include(c => c.Employees)
                .Where(c => c.Employees.Any(e => e.Id == id))
                .Include(c => c.SubscriptionPayments)
                    .ThenInclude(p => p.SubscriptionPlan)
                        .ThenInclude(p => p!.SubscriptionProduct)
                .AsNoTracking()
                .ToListAsync()! ?? new List<ClinicTable>();
        }

        return list;
    }

    public IQueryable<ClinicTable> GetAllClinicsByCompanyId(Guid id, string? searchArgument)
    {
        IQueryable<ClinicTable>? clinics;
        if (string.IsNullOrWhiteSpace(searchArgument))
        {
            clinics = context.Clinic?
                .Where(c => c.CompanyId == id)
                .OrderByDescending(c => c.Created)
                .Include(c => c.SubscriptionPayments.OrderByDescending(y => y.Created))
                .AsNoTracking()!;
        }
        else
        {
            clinics = context.Clinic?
                .Where(c => c.CompanyId == id && c.Name.Contains(searchArgument))
                .OrderByDescending(c => c.Created)
                .Include(c => c.SubscriptionPayments.OrderByDescending(y => y.Created))
                .AsNoTracking()!;
        }

        return clinics ?? Enumerable.Empty<ClinicTable>().ToList().AsQueryable();
    }

    public async Task<ClinicClient?> GetClientClinicRelation(Guid clientId, Guid clinicId)
    {
        return await context.ClinicClients?
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.ClinicId == clinicId && x.ClientId == clientId)!;
    }

    public IQueryable<ClientTable> GetClientsForClinic(Guid id, string? searchArgument = null)
    {
        IQueryable<ClientTable>? clients;
        if (string.IsNullOrWhiteSpace(searchArgument))
        {
            clients = context.Client?
                  .OrderByDescending(c => c.Created)
                  .Include(x => x.ClinicClients.Where(y => y.ClinicId == id))
                  .Where(x => x.ClinicClients.Any(y => y.ClinicId == id))
                  .AsNoTracking()!;
        }
        else
        {
            clients = context.Client?
                  .OrderByDescending(c => c.Created)
                  .Where(c => c.Name.Contains(searchArgument) || c.LastName.Contains(searchArgument) || c.Email.Contains(searchArgument))
                  .Include(x => x.ClinicClients.Where(y => y.ClinicId == id))
                  .Where(x => x.ClinicClients.Any(y => y.ClinicId == id))
                  .AsNoTracking()!;
        }

        return clients ?? Enumerable.Empty<ClientTable>().AsQueryable();
    }

    public IQueryable<ClientTable> GetClientWithClinicInformation(Guid id, string? searchArgument = null)
    {
        IQueryable<ClientTable>? clients;
        if (string.IsNullOrWhiteSpace(searchArgument))
        {
            clients = context.Client?
                   .OrderByDescending(c => c.Created)
                  .Include(x => x.ClinicClients.Where(y => y.ClinicId == id))
                  .Where(x => x.ClinicClients.Any(y => y.ClinicId == id))
                  .AsNoTracking()!;
        }
        else
        {
            clients = context.Client?
                    .OrderByDescending(c => c.Created)
                  .Where(c => c.Name.Contains(searchArgument) || c.LastName.Contains(searchArgument) || c.Email.Contains(searchArgument))
                  .Include(x => x.ClinicClients.Where(y => y.ClinicId == id))
                  .Where(x => x.ClinicClients.Any(y => y.ClinicId == id))
                  .AsNoTracking()!;
        }

        return clients ?? Enumerable.Empty<ClientTable>().AsQueryable();
    }

    public ClinicTable? GetClinic(Guid id)
    {
        return context.Clinic?
            .AsNoTracking()
            .FirstOrDefault(a => a.Id == id);
    }

    public async Task<ClinicTable?> GetClinicWithEmployeeAndClientData(Guid id)
    {
        var clinic = await context.Clinic?
            .Include(x => x.Employees)
            .Include(x => x.ClinicClients)
            .ThenInclude(x => x.Client)
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == id)!;

        return clinic;
    }

    public IQueryable<EmployeeTable> GetEmployeeForClinic(Guid id, string? searchArgument = null)
    {
        IQueryable<EmployeeTable>? employees;
        if (string.IsNullOrWhiteSpace(searchArgument))
        {
            employees = context.Employee?
                .OrderByDescending(c => c.Created)
                  .Where(x => x.Clinics.Any(y => y.Id == id))
                  .AsNoTracking()!;
        }
        else
        {
            employees = context.Employee?
                    .OrderByDescending(c => c.Created)
                  .Where(c => c.Name.Contains(searchArgument) || c.LastName.Contains(searchArgument) || c.Email.Contains(searchArgument))
                  .Where(x => x.Clinics.Any(y => y.Id == id))
                  .AsNoTracking()!;
        }

        return employees ?? Enumerable.Empty<EmployeeTable>().AsQueryable();
    }

    public IQueryable<EmployeeTable> GetEmployeesByClinic(Guid id, string? searchArgument = null)
    {
        var employees = context.Employee?
            .OrderByDescending(x => x.Created)
            .Where(x => x.Clinics.Any(y => y.Id == id))
            .AsNoTracking()!;

        return employees;
    }

    public async Task<bool> RemoveClientFromClinic(Guid clinicId, Guid clientId)
    {
        var clinic = await context.ClinicClients?
             .Where(clinic => clinic.ClinicId == clinicId)
             .Where(clinic => clinic.ClientId == clientId)
             .FirstOrDefaultAsync()!;

        if (clinic == null)
        {
            return false;
        }

        context.ClinicClients.Remove(clinic);
        await context.SaveChangesAsync();

        return true;
    }
    
    public async Task<bool> RemoveEmployeeFromClinic(Guid clinicId, Guid employeeId)
    {
        ClinicTable? clinic = await context.Clinic?
            .Include(x => x.Employees)
            .FirstOrDefaultAsync(c => c.Id == clinicId)!;

        if (clinic == null)
        {
            return false;
        }

        var employee = clinic.Employees.First(x => x.Id == employeeId);

        clinic.Employees.Remove(employee);

        await context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> Update(UpdateClinicRequest request)
    {
        var clinic = context.Clinic?
            .FirstOrDefault(a => a.Id == request.Id);

        if (clinic == null)
        {
            return false;
        }

        clinic.Name = request.Name;
        clinic.Email = request.Email;
        clinic.Adress = request.Adress;
        clinic.NIF = request.NIF;
        clinic.Phone = request.Phone;

        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateClientPlan(Guid? clinicId, Guid? clientId, Guid subscriptionPlanId)
    {
        var relation = await context.ClinicClients?
            .FirstOrDefaultAsync(x => x.ClinicId == clinicId && x.ClientId == clientId)!;

        var newPlan = await context.SubscriptionPlans?
            .Include(x => x.SubscriptionProduct)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == subscriptionPlanId)!;

        if (newPlan == null || relation == null)
        {
            return false;
        }

        relation.SubscriptionPlanSize = newPlan.SubscriptionProduct?.Size ?? 0L;
        relation.SubscriptionPlanId = newPlan.Id;

        await context.SaveChangesAsync();

        return true;
    }
}
