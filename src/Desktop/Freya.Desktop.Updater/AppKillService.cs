namespace Freya.Desktop.Updater;

public class AppKillService
{
    private readonly IHostApplicationLifetime hostApplicationLifetime;

    public AppKillService(IHostApplicationLifetime hostApplicationLifetime)
    {
        this.hostApplicationLifetime = hostApplicationLifetime;
    }

    public void Kill()
    {
        hostApplicationLifetime.StopApplication();
    }
}
