namespace FreyaApi.Repository.Tests.Services;

[TestFixture]
internal class ClinicRepositoryShould
{
    private ClinicRepositoryBuilder builder;
    private readonly string CLIENT_EMAIL = "migue@yopmail.com";
    private readonly string CLINIC_NAME = "test";
    [SetUp]
    public void SetUp()
    {
        builder = new ClinicRepositoryBuilder();
    }

    [Test]
    public async Task CanCreateClinic()
    {
        var service = builder.Build();

        var clinic = new ClinicTable()
        {
            Email = CLIENT_EMAIL,
        };

        var clientCreated = await service.Create(clinic);

        Assert.That(clientCreated, Is.True);
    }

    [Test]
    public async Task CanReadClinicById()
    {
        var service = builder.Build();


        var clinic = new ClinicTable()
        {
            Email = CLIENT_EMAIL,
            Id = Guid.NewGuid(),
        };

        await service.Create(clinic);

        var clientDatabase = service.GetClinic(clinic.Id);

        Assert.Multiple(() =>
        {
            Assert.That(clientDatabase, Is.Not.Null);
            Assert.That(clientDatabase?.Id, Is.EqualTo(clinic.Id));
            Assert.That(clientDatabase?.Email, Is.EqualTo(clinic.Email));
        });
    }
    
    [Test]
    public async Task GetClientClinicRelation()
    {
        var service = builder.Build();


        var clinicClient = new ClinicClient()
        {
            ClinicId = Guid.NewGuid(),
            ClientId = Guid.NewGuid(),
        };

        builder.dbContext.ClinicClients?.Add(clinicClient);
        builder.dbContext.SaveChanges();

        var clinicClientDatabase = await service.GetClientClinicRelation(clinicClient.ClientId, clinicClient.ClinicId);

        Assert.Multiple(() =>
        {
            Assert.That(clinicClientDatabase, Is.Not.Null);
            Assert.That(clinicClientDatabase?.ClinicId, Is.EqualTo(clinicClient.ClinicId));
            Assert.That(clinicClientDatabase?.ClientId, Is.EqualTo(clinicClient.ClientId));
        });
    }
    
    [Test]
    public async Task GetClientClinicRelationNullIfNotExist()
    {
        var service = builder
            .Build();

        var clinicClient = new ClinicClient()
        {
            ClinicId = Guid.NewGuid(),
            ClientId = Guid.NewGuid(),
        };

        var clinicClientDatabase = await service.GetClientClinicRelation(clinicClient.ClientId, clinicClient.ClinicId);

        Assert.Multiple(() =>
        {
            Assert.That(clinicClientDatabase, Is.Null);
        });
    }
    
    [Test]
    public void GetClientWithClinicInformation()
    {
        var clinic1Id = Guid.NewGuid();
        var clinic2Id = Guid.NewGuid();

        var clientId = Guid.NewGuid();

        var service = builder
            .WithClient(CLIENT_EMAIL, clientId)
            .WithClinic(CLINIC_NAME, clinic1Id)
            .WithClinic(CLINIC_NAME, clinic2Id)
            .WithClinicClinicRelation(clinic1Id, clientId)
            .WithClinicClinicRelation(clinic2Id, clientId)
            .Build();

        var clientsFromClinic = service.GetClientWithClinicInformation(clinic1Id);

        Assert.Multiple(() =>
        {
            Assert.That(clientsFromClinic.Count, Is.EqualTo(1));
        });
    }
    
    [Test]
    [TestCase("migue@")]
    [TestCase("mig")]
    [TestCase("@yopmail.com")]
    public void GetClientWithClinicInformationWithFilter(string argument)
    {
        var clinic1Id = Guid.NewGuid();
        var clinic2Id = Guid.NewGuid();

        var clientId = Guid.NewGuid();

        var service = builder
            .WithClient(CLIENT_EMAIL, clientId)
            .WithClinic(CLINIC_NAME, clinic1Id)
            .WithClinic(CLINIC_NAME, clinic2Id)
            .WithClinicClinicRelation(clinic1Id, clientId)
            .WithClinicClinicRelation(clinic2Id, clientId)
            .Build();

        var clientsFromClinic = service.GetClientWithClinicInformation(clinic1Id, argument);
        Assert.Multiple(() =>
        {
            Assert.That(clientsFromClinic.Count, Is.EqualTo(1));
        });
    }
    
    [Test]
    public void GetClientWithClinicInformation2()
    {
        var clinic1Id = Guid.NewGuid();
        var clinic2Id = Guid.NewGuid();

        var clientId = Guid.NewGuid();

        var service = builder
            .WithClient(CLIENT_EMAIL, clientId)
            .WithClinic(CLINIC_NAME, clinic1Id)
            .WithClinic(CLINIC_NAME, clinic2Id)
            .WithClinicClinicRelation(clinic1Id, clientId)
            .WithClinicClinicRelation(clinic2Id, clientId)
            .Build();

        var clientsFromClinic = service.GetClientWithClinicInformation(clinic2Id);

        Assert.Multiple(() =>
        {
            Assert.That(clientsFromClinic.Count, Is.EqualTo(1));
        });
    }
}
