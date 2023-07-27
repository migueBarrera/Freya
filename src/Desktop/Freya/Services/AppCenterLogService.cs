namespace Freya.Services;

public class AppCenterLogService : IAppCenterLogService
{
    public string LogFolderPath => LogPathHelper.LogFolderPath;
}
