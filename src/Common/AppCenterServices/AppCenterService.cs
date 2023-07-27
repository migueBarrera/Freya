using Microsoft.Extensions.Logging;

namespace AppCenterServices;

public class AppCenterService
{
    private readonly IAppCenterSecretService appCenterSecretService;
    private readonly IAppCenterLogService logFileSystemService;
    private readonly ILogger<AppCenterService> logger;

    public AppCenterService(
        IAppCenterSecretService appCenterSecretService,
        IAppCenterLogService logFileSystemService,
        ILogger<AppCenterService> logger)
    {
        if (appCenterSecretService is null)
        {
            throw new ArgumentNullException($"You must be implemented and register {nameof(IAppCenterSecretService)}");
        }

        if (logFileSystemService is null)
        {
            throw new ArgumentNullException($"You must be implemented and register {nameof(IAppCenterLogService)}");
        }

        this.appCenterSecretService = appCenterSecretService;
        this.logFileSystemService = logFileSystemService;
        this.logger = logger;
    }

    // 20/12/22 OnAndroid, we have an error if use "await AppCenter.SetEnabledAsync(true)".
    // We need use "AppCenter.SetEnabledAsync(true).Wait()"
    public async Task Initialize(bool enabled = false)
    {
        var appSecret = appCenterSecretService.GetSecret();

        logger.LogInformation("Initializing appCenter for secret {0}", appSecret);

        if (string.IsNullOrWhiteSpace(appSecret) ||
            (!appCenterSecretService.IsEnabledAnalitics && !appCenterSecretService.IsEnabledCrashes))
        {
            logger.LogInformation("Discartings initialize appcenter", appSecret);
            return;
        }

        if (enabled)
        {
            logger.LogInformation("Continue initialize AppCenter");
            AppCenter.LogLevel = Microsoft.AppCenter.LogLevel.Verbose;
            AppCenter.SetEnabledAsync(true).Wait();
            
            var services = new List<Type>();
            if (appCenterSecretService.IsEnabledAnalitics)
            {
                logger.LogInformation("Adding Service analitics");
                services.Add(typeof(Analytics));
            }

            if (appCenterSecretService.IsEnabledCrashes)
            {
                logger.LogInformation("Adding Service crashes");
                services.Add(typeof(Crashes));
            }

            if (services.Count == 0 )
            {
                logger.LogInformation("Discartigns because any service was added");
                await AppCenter.SetEnabledAsync(false);
                return;
            }

            logger.LogInformation("Call To Start AppCenter");
            AppCenter.Start(
                    appSecret,
                    services.ToArray());

            if (appCenterSecretService.IsEnabledCrashes)
            {
                logger.LogInformation("Addign GetErrorAttachments");
                Crashes.GetErrorAttachments =
                        report => GetAttatchment();
            }
        }
        else
        {
            logger.LogInformation("Discartign becasue enabled is false");
            await AppCenter.SetEnabledAsync(false);
        }
    }

    public void TrackError(Exception exception, IDictionary<string, string>? properties = null, params ErrorAttachmentLog[] attachments)
    {
        if (appCenterSecretService.IsEnabledCrashes)
        {
            Crashes.TrackError(exception, properties, attachments);
        }
    }

    public void TrackEvent(string name, IDictionary<string, string>? properties = null)
    {
        if (appCenterSecretService.IsEnabledAnalitics)
        {
            Analytics.TrackEvent(name, properties);
        }
    }

    public void GenerateTestCrash()
    {
        if (appCenterSecretService.IsEnabledCrashes)
        {
            Crashes.GenerateTestCrash();
        }
    }

    private IEnumerable<ErrorAttachmentLog> GetAttatchment()
    {
        var localFolderPath = logFileSystemService.LogFolderPath;
        var latestLogPath = Directory
            .EnumerateFiles(localFolderPath)
            .OrderByDescending(item => item)
            .FirstOrDefault();

        ErrorAttachmentLog attachment;

        if (latestLogPath == null)
        {
            attachment = ErrorAttachmentLog.AttachmentWithText("(No log found.)", "Log.txt");
        }
        else
        {
            try
            {
                var data = File.ReadAllBytes(latestLogPath);
                var fileName = Path.GetFileName(latestLogPath);
                attachment = ErrorAttachmentLog.AttachmentWithBinary(data, fileName, "text/plain");
            }
            catch (Exception e)
            {
                attachment = ErrorAttachmentLog.AttachmentWithText("(ERROR READING LOG FILE)", e.StackTrace);
            }
        }

        return new[] { attachment };
    }
}
