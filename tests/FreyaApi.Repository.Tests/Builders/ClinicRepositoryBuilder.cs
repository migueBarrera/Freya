namespace FreyaApi.Repository.Tests.Builders;

internal class ClinicRepositoryBuilder
{
    public DatabaseContext dbContext { get; private set; }

    private List<ClientTable> clients = new List<ClientTable>();
    private List<ClinicTable> clinics = new List<ClinicTable>();
    private List<ClinicClient> clinicClientRelation = new List<ClinicClient>();

    public ClinicRepositoryBuilder()
    {
        dbContext = new DatabaseBuilder().Build();
    }
    public ClinicRepository Build()
    {
        foreach (var client in clients)
        {
            dbContext.Client?.Add(client);
            dbContext.SaveChanges();
        }
        
        foreach (var clinic in clinics)
        {
            dbContext.Clinic?.Add(clinic);
            dbContext.SaveChanges();
        }

        foreach (var rel in clinicClientRelation)
        {
            dbContext.ClinicClients?.Add(rel);
            dbContext.SaveChanges();
        }

        return new ClinicRepository(dbContext);
    }

    internal ClinicRepositoryBuilder WithClient(string email, Guid id)
    {
        clients.Add(new ClientTable()
        {
            Email = email,
            Id = id,
        });

        return this;
    }

    internal ClinicRepositoryBuilder WithClinic(string clinicName, Guid id)
    {
        clinics.Add(new ClinicTable()
        {
            Name = clinicName,
            Id = id,
        });

        return this;
    }

    internal ClinicRepositoryBuilder WithClinicClinicRelation(Guid clinic1Id, Guid clientId)
    {
        clinicClientRelation.Add(new ClinicClient()
        {
            ClientId = clientId,
            ClinicId = clinic1Id,
        });

        return this;
    }
}
