namespace Freya.Desktop.Updater.Tests;


public class AppVersionServiceBuilder
{
    public AppVersionService Build()
    {
        var appMock = new Mock<Application>();
        return new AppVersionService(appMock.Object);
    }

    public static Mock<AppVersionService> BuildMock()
    {
        return new Mock<AppVersionService>(null);
    }
}
