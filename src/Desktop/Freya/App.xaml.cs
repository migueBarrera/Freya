using Microsoft.AppCenter.Crashes;
using System.Windows.Threading;

namespace Freya;

public partial class App : Application
{
    public event EventHandler? Loaded;

    public IHost? AppHost { get; private set; }

    protected override async void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        this.AppHost = Host.CreateDefaultBuilder(e.Args)
                .ConfigureAppConfiguration((context, builder) => builder.ConfigureAppConfigurationFile())
                .ConfigureServices((context, services) => services.ConfigureHostAndAppServices(this, context.Configuration))
                .ConfigureLogging((context, builder) => builder.ConfigureLogService(context.Configuration))
                .Build();

        await this.AppHost.StartAsync();

        this.DispatcherUnhandledException += UnhandledException;

        this.Loaded?.Invoke(this, EventArgs.Empty);
        this.Loaded = null;

        await this.AppHost.WaitForShutdownAsync();
        this.Shutdown();
    }

    private void UnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        Crashes.TrackError(e.Exception);
        this.Shutdown();
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        base.OnExit(e);
        if (this.AppHost != null)
        {
            await this.AppHost.StopAsync();
            this.AppHost.Dispose();
        }
    }
}
