using System;

namespace FreyaMobile.Core.Services;

public class ReleaseLoggingService : ILoggingService
{
    private readonly AppCenterService appCenterService;
    private readonly IFileLoggingService fileLoggingService;
    private readonly IFirebaseAnalytics firebaseAnalytics;

    public ReleaseLoggingService(
        AppCenterService appCenterService,
        IFileLoggingService fileLoggingService,
        IFirebaseAnalytics firebaseAnalytics)
    {
        this.appCenterService = appCenterService;
        this.fileLoggingService = fileLoggingService;

        Initialize();
        this.firebaseAnalytics = firebaseAnalytics;
    }

    public void Initialize()
    {
        appCenterService.Initialize(true).Wait();
        fileLoggingService.Init();
    }

    public void Debug(string message)
    {
        appCenterService.TrackEvent(message); 
        fileLoggingService.LogInfo(message);
        firebaseAnalytics.LogEvent(message);
    }

    public void Error(Exception exception)
    {
        appCenterService.TrackError(exception);
        fileLoggingService.LogError($"{nameof(Error)}", exception);
        firebaseAnalytics.LogEvent("Exception", new Dictionary<string, object?>()
        {
            { "Message", exception.Message },
            { "StackTrace", exception.StackTrace ?? string.Empty },
            { "InnerException", exception.InnerException},
        });
    }

    public void Warning(string message)
    {
        appCenterService.TrackEvent(message, null);
        fileLoggingService.LogWarning(message);
        firebaseAnalytics.LogEvent("Warning", new Dictionary<string, object>()
        {
            { "Message", message },
        });
    }
}
