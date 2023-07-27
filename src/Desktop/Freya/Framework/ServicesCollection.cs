using Freya.Desktop.Core.Features.Clients;
using Freya.Desktop.Core.Features.Clients.Models;
using Freya.Desktop.Core.Features.Clients.Services;
using Freya.Desktop.Core.Features.Clinics;
using Freya.Desktop.Core.Features.Dialogs;
using Freya.Desktop.Core.Features.SubscriptionCharges;

namespace Freya.Framework;

internal static class ServicesCollection
{
    internal static void InternalConfigureAppCenter(this IServiceCollection serviceProvider)
    {
        serviceProvider.UseAppCenter<AppCenterLogService, AppCenterSecretService>();
        serviceProvider.AddTransient<IHostedService, AppCenterHostedService>();
    }
    
    internal static void ConfigureAppConfigurationFile(this IConfigurationBuilder builder)
    {
        builder.AddJsonFile("app.settings.json", optional: true, reloadOnChange: true)
                           .AddEnvironmentVariables();
    }
    
    internal static void ConfigureLogService(this ILoggingBuilder builder, IConfiguration configuration)
    {
        var logPath = LogPathHelper.LogFilePath;

        var logger = new LoggerConfiguration()
                       .MinimumLevel.Information()
                       .WriteTo.File(logPath, rollingInterval: RollingInterval.Day, shared: true)
                       .CreateLogger();

        builder.AddDebug();
        builder.AddSerilog(logger);
    }

    internal static IServiceCollection ConfigureHostAndAppServices(this IServiceCollection serviceProvider, Application application, IConfiguration configuration)
    {
        serviceProvider.AddHttpClient(); //https://docs.microsoft.com/es-es/aspnet/core/fundamentals/http-requests?view=aspnetcore-6.0 investigar como implementar
        serviceProvider.Configure<HostOptions>(o => o.ShutdownTimeout = TimeSpan.FromSeconds(30));

        serviceProvider.ConfigureAppServices();
        serviceProvider.AddSingleton<IHostedService>(x => x.GetRequiredService<MainWindow>());
        serviceProvider.ConfigureWasabiUploaderServices(configuration);
        serviceProvider.AddSingleton<MainWindow>();
        serviceProvider.AddSingleton(application);

        return serviceProvider;
    }
    
    internal static IServiceCollection ConfigureAppServices(this IServiceCollection serviceProvider)
    {
        ConfigureApiContractServices(serviceProvider);
        ConfigureCommonServices(serviceProvider);
        ConfigureInternalServices(serviceProvider);
        ConfigureViewModel(serviceProvider);
        ConfigurePages(serviceProvider);
        InternalConfigureAppCenter(serviceProvider);
        serviceProvider.UseDesktopCoreDialogs<DialogWindowsService>();
        serviceProvider.UseDesktopUpdater();

        return serviceProvider;
    }

    private static void ConfigureApiContractServices(IServiceCollection services)
    {
        services.AddTransient<IAppSecretsService, AppSecretsService>();
        services.AddTransient<IRefitService, RefitService>();
        services.AddSingleton<IAuthTokenService, AuthTokenService>();
    }

    private static void ConfigureCommonServices(IServiceCollection services)
    {
        services.AddSingleton<SessionHelper>();
        services.AddSingleton<ISessionService, SessionService>();
        services.AddTransient<IConnectivityService, ConnectivityService>();
        services.AddTransient<ITaskHelper, TaskHelper>();
        services.AddTransient<ITaskHelperFactory, TaskHelperFactory>();
        services.AddTransient<IEmployeeRolService, EmployeeRolService>();
        services.AddTransient<ITranslatorService, TranslatorService>();
        services.AddTransient<SubscriptionChargesService>();
        services.AddTransient<IEditClinicService, EditClinicService>();
        services.AddTransient<MultimediaService>();
    }

    private static void ConfigureInternalServices(IServiceCollection services)
    {
        services.AddSingleton<IEditEmployeeService, EditEmployeeService>();
        services.AddSingleton<ISessionManagerService, SessionManagerService>();
        services.AddSingleton<ICurrentClinicService, CurrentClinicService>();
        services.AddSingleton<ICurrentEmployeeService, CurrentEmployeeService>();
        services.AddSingleton<INavigationService, NavigationService>();
        services.AddTransient<ISignInService, SignInService>();
        services.AddTransient<IEmployeeService, EmployeeService>();
        services.AddTransient<ICheckEmployeeService, CheckEmployeeService>();
        services.AddTransient<ICreateNewEmployeeService, CreateNewEmployeeService>();
        services.AddTransient<ClientService>();
        services.AddTransient<ICheckClientService, CheckClientService>();
        services.AddTransient<ICreateClientService, CreateClientService>();
        services.AddTransient<IClinicsService, ClinicsService>();
        services.AddTransient<ICreateNewClinicService, CreateNewClinicService>();
        services.AddTransient<IClientDetailService, ClientDetailService>();
        services.AddTransient<ProfileService>();
        services.AddTransient<FaqService>();
        services.AddTransient<EditClientService>();
        services.AddTransient<IClinicDetailService, ClinicDetailService>();
        services.AddTransient<IMultimediaFilePickerService, MultimediaFilePickerService>();
        services.AddTransient<SessionViewModel>();

    }

    private static void ConfigureViewModel(IServiceCollection services)
    {
        services.AddSingleton<MainViewModel>();
        services.AddTransient<LateralMenuViewModel>();
        services.AddTransient<HeaderControlViewModel>();
        services.AddTransient<SignInViewModel>();
        services.AddTransient<ClinicsViewModel>();
        services.AddTransient<EditClinicViewModel>();
        services.AddTransient<ClientsViewModel>();
        services.AddTransient<CheckEmployeeViewModel>();
        services.AddTransient<CreateNewEmployeeViewModel>();
        services.AddTransient<CheckClientViewModel>();
        services.AddTransient<CreateNewClinicViewModel>();
        services.AddTransient<CreateNewClientViewModel>();
        services.AddTransient<ClientDetailViewModel>();
        services.AddTransient<SoundMultimediaViewModel>();
        services.AddTransient<ImageMultimediaViewModel>();
        services.AddTransient<VideoMultimediaViewModel>();
        services.AddTransient<CheckoutViewModel>();
        services.AddTransient<ClinicDetailViewModel>();
        services.AddTransient<SettingViewModel>();
        services.AddTransient<ClinicItemViewModel>();
        services.AddTransient<SubscriptionProductItemViewModel>();
        services.AddTransient<ClientItemViewModel>();
        services.AddTransient<EmployeeItemViewModel>();
        services.AddTransient<EditEmployeeViewModel>();
        services.AddTransient<EditClientViewModel>();
        services.AddTransient<ProfileViewModel>();
        services.AddTransient<ChangePassDialog>();
        services.AddTransient<ChangePassDialogViewModel>();
        services.AddTransient<MultimediaDialogViewModel>();
        services.AddTransient<FaqViewModel>();
        services.AddTransient<FaqItemViewModel>();
        services.AddTransient<ClinicDetailEmployeesViewModel>();
        services.AddTransient<ClinicDetailClientsViewModel>();
        services.AddTransient<SubscriptionPaymentDetailViewModel>();
        services.AddTransient<SubscriptionClinicItemViewModel>();
    }

    private static void ConfigurePages(IServiceCollection services)
    {
        services.AddTransient<SignInPage>();
        services.AddTransient<ClinicsPage>();
        services.AddTransient<EditClinicPage>();
        services.AddTransient<ClientsPage>();
        services.AddTransient<CheckEmployeePage>();
        services.AddTransient<CreateNewEmployeePage>();
        services.AddTransient<CheckClientPage>();
        services.AddTransient<CreateNewClientPage>();
        services.AddTransient<CreateNewClinicPage>();
        services.AddTransient<ClientDetailPage>();
        services.AddTransient<CheckoutPage>();
        services.AddTransient<ClinicDetailPage>();
        services.AddTransient<SettingPage>();
        services.AddTransient<EditEmployeePage>();
        services.AddTransient<EditClientPage>();
        services.AddTransient<ProfilePage>();
        services.AddTransient<FaqPage>();
        services.AddTransient<SubscriptionPaymentDetailPage>();
    }
}
