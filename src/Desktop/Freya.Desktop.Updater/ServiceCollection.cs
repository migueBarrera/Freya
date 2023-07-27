using Microsoft.Extensions.DependencyInjection;

namespace Freya.Desktop.Updater;

public static class ServiceCollection
{
    public static void UseDesktopUpdater(this IServiceCollection serviceProvider)
    {
        serviceProvider.AddTransient<AppVersionService>();
        serviceProvider.AddTransient<AppKillService>();
        serviceProvider.AddTransient<DownloaderService>();
        serviceProvider.AddTransient<ProcessRunner>();
        serviceProvider.AddTransient<IAppUpdaterManagerService, AppUpdaterManagerService>();
        serviceProvider.AddTransient<AppUpdaterService>();
        serviceProvider.AddTransient<UploadingDialogViewModel>();
        serviceProvider.AddTransient<UploadingDialog>();
    }
}
