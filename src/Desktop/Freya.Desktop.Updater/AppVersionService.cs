namespace Freya.Desktop.Updater;

public class AppVersionService
{
    private Version defaultVersion = new Version(1, 0, 0, 0);

    private readonly Application application;

    public AppVersionService(Application application)
    {
        this.application = application;
    }

    public virtual Version GetAppVersion()
    {
        if (application?.MainWindow == null)
        {
            return defaultVersion;
        }

        var assembly = Assembly.GetAssembly(application.MainWindow.GetType());
        var version = assembly?.GetName().Version;
        return version ?? defaultVersion;
    }

    public virtual bool IsThisADebugVersion()
    {
        var assembly = Assembly.GetAssembly(application.MainWindow.GetType());
        var version = assembly?.GetName().Version;

        return version?.Equals(defaultVersion) ?? true;
    }
}
