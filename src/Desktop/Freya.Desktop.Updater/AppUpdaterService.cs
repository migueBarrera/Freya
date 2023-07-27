using System.Net.Http;
using System.IO;
using AppCenterServices;

namespace Freya.Desktop.Updater;

public class AppUpdaterService
{
    private readonly HttpClient http;
    private readonly string updateUrl;
    private readonly AppVersionService appVersionService;
    private readonly DownloaderService downloaderService;
    private readonly ProcessRunner processRunner;
    private readonly AppKillService appKillService;
    private readonly ILogger<AppUpdaterService> logger;
    private readonly IDialogService dialogService;
    private readonly AppCenterService appCenterService;

    public AppUpdaterService(
        AppVersionService appVersionService,
        IConfiguration configuration,
        ILogger<AppUpdaterService> logger,
        DownloaderService downloaderService,
        ProcessRunner processRunner,
        AppKillService appKillService,
        IDialogService dialogService,
        AppCenterService appCenterService)
    {
        http = new HttpClient();
        updateUrl = configuration.GetValue<string>("Data:AppUpdateUrl") ?? string.Empty;
        this.logger = logger;
        this.appVersionService = appVersionService;
        this.downloaderService = downloaderService;
        this.processRunner = processRunner;
        this.appKillService = appKillService;
        this.dialogService = dialogService;
        this.appCenterService = appCenterService;
    }

    public async Task<bool> IsThereAnUpdate()
    {
        if (appVersionService.IsThisADebugVersion())
        {
            logger.LogInformation("Discarting update because is a debug session");

            return false;
        }

        var currentVersion = appVersionService.GetAppVersion();
        try
        {
            var timeoutCancellationToken = new CancellationTokenSource();
            timeoutCancellationToken.CancelAfter(TimeSpan.FromSeconds(5));
            var httpResponse = await http.GetAsync(updateUrl, HttpCompletionOption.ResponseHeadersRead, timeoutCancellationToken.Token).ConfigureAwait(false);
            if (TryParseHeaderToVersion(httpResponse.Headers, out var version))
            {
                logger.LogInformation("Current version: {0}", currentVersion);
                logger.LogInformation("Last version: {0}", version);
                var updatedRequired = currentVersion < version;
                logger.LogInformation(updatedRequired ? "The application will be updated" : "No update required, last version installed");

                return updatedRequired;
            }
        }
        catch (Exception ex)
        {
            appCenterService.TrackError(ex);
            logger.LogError(ex, $"Exception checking if there are a new version available");
        }

        return false;
    }

    private static bool TryParseHeaderToVersion(HttpResponseHeaders headers, out Version version)
    {
        version = new Version(1, 0, 0);

        if (headers.TryGetValues("x-ms-meta-AppVersion", out var versionValues))
        {
            if (Version.TryParse(versionValues.First(), out version!))
            {
                return true;
            }
        }

        return false;
    }

    public async Task<bool> Update(Progress<float> progressUpdater)
    {
        bool installerExecuted = false;

        try
        {
            var downloadedFilaPath = await downloaderService.Download(http, updateUrl, progressUpdater);
            if (!File.Exists(downloadedFilaPath))
            {
                return false;
            }

            var tempUpdateExtractFolder = Directory.CreateDirectory(Path.Combine(Path.GetTempPath(), Path.GetFileNameWithoutExtension(downloadedFilaPath)));
            ZipFile.ExtractToDirectory(downloadedFilaPath, tempUpdateExtractFolder.FullName);

            var file = Directory.GetFiles(tempUpdateExtractFolder.FullName).FirstOrDefault();
            if (!File.Exists(file))
            {
                return false;
            }

            installerExecuted = processRunner.Run(file, makeSilent: false);
            if (installerExecuted)
            {
                appKillService.Kill();
            }
        }
        catch (Exception ex)
        {
            appCenterService.TrackError(ex);
            logger.LogError(ex, $"Exception trying download new version");
        }
        finally
        {
            dialogService.CloseAll();
        }

        return installerExecuted;
    }
}
