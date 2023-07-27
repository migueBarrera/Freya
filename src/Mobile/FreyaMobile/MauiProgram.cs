namespace FreyaMobile;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureAppConfiguration()
            .ConfigureEssentials()
            .CoreUseAppCenter<AppCenterLogService, AppCenterSecretService>()
            .ConfigureCoreServices()
            .ConfigureApiContractServices()
            .ConfigureServices()
            .ConfigureViewModels()
            .ConfigurePages()
            .ConfigureNativeServices()
            .RegisterShellRoutes()
            .UseMauiCommunityToolkit()
            .UseMauiCommunityToolkitMediaElement()
            .RegisterFirebaseServices()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("RobotoBold.ttf", "RobotoBold");
                fonts.AddFont("Roboto-BoldItalic.ttf", "RobotoBoldItalic");
                fonts.AddFont("Roboto-Italic.ttf", "RobotoItalic");
                fonts.AddFont("Roboto-Medium.ttf", "RobotoMedium");
                fonts.AddFont("Roboto-MediumItalic.ttf", "RobotoMediumItalic");
                fonts.AddFont("Roboto-Regular.ttf", "RobotoRegular");
            });

        return builder.Build();
    }
}