using Amazon.S3.Model;

namespace FreyaApi.Tests.Controllers.Services;

internal class CompaniesControllerServiceShould
{
    private CompaniesControllerServiceBuilder builder;

    [SetUp]
    public void Init()
    {
        builder = new CompaniesControllerServiceBuilder();
    }

    [Test]
    [TestCase(5, 1, 5)]
    [TestCase(5, 2, 1)]
    [TestCase(5, 3, 0)]
    public void GetCompaniesFilters(int pageSize, int pageNumber, int numbersItemsExpected)
    {
        var controlerBase = new ControllerBaseBuilder().Build();
        var service = builder
            .WithCompany("Test")
            .WithCompany("Test2")
            .WithCompany("Test3")
            .WithCompany("Test4")
            .WithCompany("Test5")
            .WithCompany("Test6")
            .Build();

        var filter = new PaginationFilter()
        {
            PageSize = pageSize,
            PageNumber = pageNumber,
        };

        var response = service.Index(controlerBase, filter);
        var resultObject = response.GetObjectResult();

        Assert.Multiple(() =>
        {
            Assert.That(resultObject.Items.Count(), Is.EqualTo(numbersItemsExpected));
            Assert.That(response.Result, Is.InstanceOf<OkObjectResult>());
        });
    }

    [Test]
    public async Task Can_Delete_A_Company()
    {
        var id = Guid.NewGuid();
        var controlerBase = new ControllerBaseBuilder().Build();
        var service = builder
            .WithCompany(id, "Test")
            .Build();

        var response = await service.DeleteCompany(controlerBase, id);

        Assert.Multiple(() =>
        {
            Assert.That(response, Is.InstanceOf<OkObjectResult>());
        });
    }
    
    [Test]
    public async Task Can_Not_Delete_A_Company_If_Has_Clinic()
    {
        var id = Guid.NewGuid();
        var idClinic = Guid.NewGuid();
        var controlerBase = new ControllerBaseBuilder().Build();
        var service = builder
            .WithCompany(id, "Test")
            .WithClinic(id, idClinic, "TestClinic")
            .Build();

        var response = await service.DeleteCompany(controlerBase, id);

        Assert.Multiple(() =>
        {
            Assert.That(response, Is.InstanceOf<ConflictResult>());
        });
    }
}
