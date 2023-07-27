using Microsoft.EntityFrameworkCore;
using Models.Core.Clients;

namespace FreyaApi.Repository.Services;

public class ClientRepository : BaseRepository, IClientRepository
{
    public ClientRepository(DatabaseContext context) 
        : base(context)
    {
    }

    public Task<ClientTable?> GetClientByEmail(string email)
    {
        return context.Client?
            .Where(a => a.Email == email)
            .AsNoTracking()
            .FirstOrDefaultAsync()!;
    }

    public async Task<IEnumerable<ClinicTable>> GetClinicsForClient(Guid id)
    {
        var clinis = await context.ClinicClients?
            .Include(x => x.Clinic)
            .Where(x => x.ClientId == id)
            .AsNoTracking()
            .ToListAsync()!;

        return clinis?.Select(c => new ClinicTable()
        {
            Id = c.ClinicId,
            CompanyId = c.Clinic?.CompanyId ?? Guid.Empty,
            Name = c.Clinic?.Name ?? string.Empty,
        }) ?? Enumerable.Empty<ClinicTable>();
    }

    

    public bool ClientExist(string email)
    {
        return context.Client?.Any(e => e.Email == email) ?? false;
    }

    public bool ClientExist(Guid id)
    {
        return context.Client?.Any(e => e.Id == id) ?? false;
    }

    public bool ClienHasIncludeOnAClinic(string clientEmail, Guid clinicId)
    {
        var employee = context.Client?
           .Where(employee => employee.Email == clientEmail)
           .Include(c => c.ClinicClients)
           .AsNoTracking()
           .FirstOrDefault();

        var employeeIsAsignedForClinic = employee?.ClinicClients?.Any(c => c.ClinicId == clinicId) ?? false;

        return employeeIsAsignedForClinic;
    }

    public async Task<bool> UpdatePass(string email, string pass)
    {
        var client = await context.Client?
            .FirstOrDefaultAsync(a => a.Email == email)!;

        if (client == null)
        {
            return false;
        }

        client.Pass = pass;
        await context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> UpdatePass(Guid clientId, string pass)
    {
        var client = await context.Client?
            .FirstOrDefaultAsync(a => a.Id == clientId)!;

        if (client == null)
        {
            return false;
        }

        client.Pass = pass;
        await context.SaveChangesAsync();

        return true;
    }

    public ClientTable? GetClientById(Guid clientId)
    {
        return context.Client?
            .Where(a => a.Id == clientId)
            .AsNoTracking()
            .FirstOrDefault();
    }

    public async Task<ClientTable?> Create(ClientTable client)
    {
        context.Client?.Add(client);

        await context.SaveChangesAsync();

        return client;
    }

    public async Task<bool> UpdateClient(Guid id, ClientUpdateRequest request)
    {
        var client = await context.Client?
           .Where(a => a.Id == id)
           .FirstOrDefaultAsync()!;

        if (client == null)
        {
            return false;
        }

        client.Name = request.Name;
        client.LastName = request.LastName;
        client.Phone = request.Phone;

        await context.SaveChangesAsync();
        return true;
    }

    public ClientTable? GetClientByIdWithClinicRelation(Guid clientId, Guid clinicId)
    {
        var client = context.Client?
            .Include(c => c.ClinicClients)
            .AsNoTracking()
            .FirstOrDefault(a => a.Id == clientId)!;

        if (client == null)
        {
            return null;
        }

        client.ClinicClients = client.ClinicClients.Where(cc => cc.ClinicId == clinicId).ToList();

        return client;
    }

    public async Task<Guid?> GetClientIdByEmail(string email)
    {
        var id = await context.Client?
            .Where(a => a.Email == email)
            .AsNoTracking()
            .Select(x => x.Id)
            .FirstOrDefaultAsync()!;

        return id;
    }

    public Task<long> GetSizeFromClientAndClinic(Guid clientId, Guid clinicId)
    {
        var result = (from c in context.Client
                      where c.Id == clientId
                      select new
                      {
                          TotalSize = context.Videos!.Where(x => x.ClientId == clientId && x.ClinicId == clinicId).Sum(x => x.Size) +
                                      context.Sounds!.Where(x => x.ClientId == clientId && x.ClinicId == clinicId).Sum(x => x.Size) +
                                      context.Ultrasounds!.Where(x => x.ClientId == clientId && x.ClinicId == clinicId).Sum(x => x.Size)
                      }).SingleOrDefault();

        return Task.FromResult(result?.TotalSize ?? 0);  
    }
}
