using AppCenterServices;

namespace Freya.Desktop.Updater.Tests;

internal class AppUpdaterServiceBuilder
{
    private Mock<AppVersionService> appVersionService;
    private Mock<DownloaderService> downloaderService;
    private Mock<ProcessRunner> processRunner;
    private Mock<AppKillService> appKillService;
    private Mock<IConfiguration> configuration;
    private Mock<IDialogService> dialogService;
    private Mock<AppCenterService> appCenterService;
    private ILogger<AppUpdaterService> logger;

    private bool isADebugSession = false;
    private Version version = new Version(1, 0, 0);

    private static string URL_UPDATE = "https://mibebetestmbmdevelop.blob.core.windows.net/production/Freya.zip";

    public AppUpdaterServiceBuilder()
    {
        appVersionService = AppVersionServiceBuilder.BuildMock();
        logger = new NullLogger<AppUpdaterService>();
        downloaderService = DownloaderServiceBuilder.BuildMock();
        processRunner = ProcessRunnerBuilder.BuildMock();
        appKillService = AppKillServiceBuilder.BuildMock();
        configuration = new Mock<IConfiguration>();
        dialogService = new Mock<IDialogService>();
        appCenterService = new Mock<AppCenterService>(
            new Mock<IAppCenterSecretService>().Object, 
            new Mock<IAppCenterLogService>().Object, 
            new NullLogger<AppCenterService>());

        var configurationSection = new Mock<IConfigurationSection>();
        configurationSection.Setup(x => x.Value).Returns(URL_UPDATE);
        configuration.Setup(x => x.GetSection(It.Is<string>(k => k == "Data:AppUpdateUrl"))).Returns(configurationSection.Object);
    }

    public AppUpdaterService Build()
    {
        appVersionService.Setup(x => x.IsThisADebugVersion()).Returns(isADebugSession);
        appVersionService.Setup(x => x.GetAppVersion()).Returns(version);

        return new AppUpdaterService(
            appVersionService.Object,
            configuration.Object,
            logger,
            downloaderService.Object,
            processRunner.Object,
            appKillService.Object,
            dialogService.Object,
            appCenterService.Object);
    }

    internal AppUpdaterServiceBuilder WithCurrentVersion(int mayor, int minor, int build)
    {
        version = new Version(mayor, minor, build);
        return this;
    }

    internal AppUpdaterServiceBuilder WithDebugSession(bool isADebugSession)
    {
        this.isADebugSession = isADebugSession;
        return this;
    }
}
