using Freya.Desktop.Updater;
using System.Diagnostics;

namespace Freya.Desktop.Core.Services;

public class AppCenterHostedService : IHostedService
{
    private readonly AppCenterService appCenterService;
    private readonly AppVersionService appVersionService;
    private readonly ILogger<AppCenterHostedService> logger;

    public AppCenterHostedService(
        AppCenterService appCenterService,
        ILogger<AppCenterHostedService> logger,
        AppVersionService appVersionService)
    {
        this.appCenterService = appCenterService;
        this.logger = logger;
        this.appVersionService = appVersionService;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        bool activateAppCenter = true;
        if (Debugger.IsAttached)
        {
            activateAppCenter = false;
        }

        logger.LogInformation("Start AppCenterHostedService to {0}", activateAppCenter);
        await appCenterService.Initialize(activateAppCenter);

        if (activateAppCenter)
        {
            appCenterService.TrackEvent(Events.APP_START, TryGetData());
        }
    }

    private IDictionary<string, string> TryGetData()
    {
        try
        {
            var dic = new Dictionary<string, string>()
            {
                { "version", appVersionService.GetAppVersion().ToString() },
                { "is64BitProcess", Environment.Is64BitProcess.ToString() },
                { "is64BitOperatingSystem", Environment.Is64BitOperatingSystem.ToString() },
            };

            return dic;
        }
        catch (Exception)
        {
            return new Dictionary<string, string>();
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
