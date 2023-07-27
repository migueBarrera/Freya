namespace FreyaApi.Repository.Tests.Services;

[TestFixture]
internal class CompaniesRepositoryShould
{
    private CompaniesRepositoryBuilder builder;

    [SetUp]
    public void SetUp()
    {
        builder = new CompaniesRepositoryBuilder();
    }

    [Test]
    public async Task Can_Create_A_Company()
    {
        var service = builder.Build();

        var company = new CompanyTable()
        {
            Name = "Test",
        };
        bool created = await service.CreateCompany(company);

        Assert.That(created, Is.True);
    }
    
    [Test]
    public void Get_Companies()
    {
        var service = builder
            .WithCompany("Test1")
            .WithCompany("Test2")
            .Build();

        var list = service.GetCompanies();

        Assert.That(list.Count(), Is.EqualTo(2));
    }
    
    [Test]
    public async Task GetCompaniesWithEmployeeAndClinicDataById()
    {
        var companyId = Guid.NewGuid();

        var service = builder
            .WithCompany("Test1", companyId)
            .Build();

        var companyTable = await service.GetCompanyById(companyId);

        Assert.That(companyTable?.Id, Is.EqualTo(companyId));
    }

    [Test]
    public async Task Can_Delete_A_Company()
    {
        var service = builder.Build();

        var company = new CompanyTable()
        {
            Name = "Test",
        };
        bool created = await service.CreateCompany(company);

        var deleted = await service.DeleteCompany(company.Id);
        Assert.Multiple(() =>
        {
            Assert.That(created, Is.True);
            Assert.That(deleted, Is.True);
        });
    }
    
    [Test]
    public async Task Can_Not_Delete_A_Company_If_Not_Exist()
    {
        var service = builder.Build();

        var company = new CompanyTable()
        {
            Name = "Test",
        };

        var deleted = await service.DeleteCompany(company.Id);
        Assert.Multiple(() =>
        {
            Assert.That(deleted, Is.False);
        });
    }
}
