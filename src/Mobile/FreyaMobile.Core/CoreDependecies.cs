using Freya.Mobile.Firebase;
using Freya.Mobile.Firebase.Analytics;
using Microsoft.Maui.LifecycleEvents;

namespace FreyaMobile.Core;

public static class CoreDependecies
{
    public static MauiAppBuilder RegisterFirebaseServices(this MauiAppBuilder builder)
    {
#if !DEBUG
            builder.ConfigureLifecycleEvents(events => {
    #if IOS
                events.AddiOS(iOS => iOS.FinishedLaunching((app, launchOptions) => {
                    CrossFirebase.Initialize(app, launchOptions, CreateCrossFirebaseSettings());
                    return false;
                }));
    #else
                events.AddAndroid(android => android.OnCreate((activity, state) =>
                    CrossFirebase.Initialize(activity, state, CreateCrossFirebaseSettings())));
    #endif
            });

            builder.Services.AddSingleton(_ => CrossFirebaseAnalytics.Current);
#endif

        return builder;
    }

    private static CrossFirebaseSettings CreateCrossFirebaseSettings()
    {
        return new CrossFirebaseSettings(isAnalyticsEnabled: true);
    }

    public static MauiAppBuilder ConfigureCoreServices(this MauiAppBuilder builder)
    {
        var serviceCollection = builder.Services;

        serviceCollection.AddTransient<DialogService>();
        serviceCollection.AddTransient<IConnectivityService, ConnectivityService>();
        serviceCollection.AddTransient<IGeolocationService, GeolocationService>();
        serviceCollection.AddTransient<IGeocodingService, GeocodingService>();
        serviceCollection.AddTransient<IFileLoggingService, FileLoggingService>();
        serviceCollection.AddTransient<ITaskHelper, TaskHelper>();
        serviceCollection.AddTransient<ITaskHelperFactory, TaskHelperFactory>();
        serviceCollection.AddTransient<IDownloaderService, DownloaderService>();
        serviceCollection.AddTransient<IShareService, ShareService>();
        
        serviceCollection.AddSingleton<ISessionService, SessionService>();

#if DEBUG
        serviceCollection.AddSingleton<ILoggingService, DebugLoggingService>();
#else
        serviceCollection.AddSingleton<ILoggingService, ReleaseLoggingService>();
#endif

        return builder;
    }

    public static MauiAppBuilder CoreUseAppCenter<AppCenterLogImplementation, AppCenterSecretsImplementation>(this MauiAppBuilder builder)
        where AppCenterLogImplementation : class, IAppCenterLogService
        where AppCenterSecretsImplementation : class, IAppCenterSecretService
    {
        var serviceProvider = builder.Services;

        serviceProvider.UseAppCenter<AppCenterLogImplementation, AppCenterSecretsImplementation>();

        return builder;
    }
}
