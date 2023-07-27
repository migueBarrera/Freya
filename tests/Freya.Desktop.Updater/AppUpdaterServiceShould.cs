namespace Freya.Desktop.Updater.Tests;

[TestFixture]
public class AppUpdaterServiceShould
{
    private AppUpdaterServiceBuilder builder;

    [SetUp]
    public void Setup()
    {
        builder = new AppUpdaterServiceBuilder();
    }

    [Test]
    public async Task IsThereAnUpdate()
    {
        var service = builder
            .Build();

        var isThereAnUpdate = await service.IsThereAnUpdate();

        Assert.That(isThereAnUpdate, Is.True);
    }

    [Test]
    public async Task NoIsThereAnUpdateIfIsADebugSession()
    {
        var service = builder
            .WithDebugSession(true)
            .Build();

        var isThereAnUpdate = await service.IsThereAnUpdate();

        Assert.That(isThereAnUpdate, Is.False);
    }

    [Test]
    public async Task NoIsThereAnUpdateIfIsHasMayorVersion()
    {
        var service = builder
            .WithCurrentVersion(9, 0, 0)
            .Build();

        var isThereAnUpdate = await service.IsThereAnUpdate();

        Assert.That(isThereAnUpdate, Is.False);
    }
}
