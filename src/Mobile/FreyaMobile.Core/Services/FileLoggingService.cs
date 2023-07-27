namespace FreyaMobile.Core.Services;

internal class FileLoggingService : IFileLoggingService
{
    public const string LogTag = "FreyaMobile";
    private const string LogTemplateFilename = "log.txt";

    public FileLoggingService()
    {
    }

    public void Init()
    {
        var logFilePath = Path.Combine(LogPathHelper.LogFolderPath, LogTemplateFilename);

        Log.Logger = new LoggerConfiguration()
            //.WriteTo.AndroidLog()
            .Enrich.WithProperty(Serilog.Core.Constants.SourceContextPropertyName, LogTag)
            .WriteTo.File(
                logFilePath,
                rollingInterval: RollingInterval.Day,
                rollOnFileSizeLimit: true,
                retainedFileCountLimit: 110)
            .CreateLogger();

        LogInfo($"{nameof(FileLoggingService)} init; path: {logFilePath}");
    }

    public void LogInfo(string message)
    {
        Log.Information(message);
    }

    public void LogWarning(string message)
    {
        Log.Warning(message);
    }

    public void LogError(string message, Exception? ex = null, IDictionary<string, string>? properties = null)
    {
        Log.Error(ex, message, properties);
    }
}
