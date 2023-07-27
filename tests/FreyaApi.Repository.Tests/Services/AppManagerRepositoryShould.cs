namespace FreyaApi.Repository.Tests.Services;

[TestFixture]
internal class AppManagerRepositoryShould
{
    private AppManagerRepositoryBuilder builder;
    private string TEST_EMAIL = "miguel@gmail.com";
    private string TEST_ALTERNATIVE_EMAIL = "pepe@gmail.com";

    [SetUp]
    public void Setup()
    {
        builder = new AppManagerRepositoryBuilder();
    }

    [Test]
    public async Task CanInsertOneAppManager()
    {
        var service = builder.Build();

        var appManager = new AppManagerTable()
        {
            Email = TEST_EMAIL,
            Pass = "XXX",
        };

        await service.Create(appManager);

        var managerDb = service.GetAppManager(TEST_EMAIL);

        Assert.That(managerDb, Is.Not.Null);
        Assert.That(managerDb.Email, Is.EqualTo(appManager.Email));
    }
    
    [Test]
    public async Task ReturnNullIfAppManagerNotExist()
    {
        var service = builder.Build();

        var appManager = new AppManagerTable()
        {
            Email = TEST_EMAIL,
            Pass = "XXX",
        };

        await service.Create(appManager);

        var managerDb = service.GetAppManager(TEST_ALTERNATIVE_EMAIL);

        Assert.That(managerDb, Is.Null);
    }
}
