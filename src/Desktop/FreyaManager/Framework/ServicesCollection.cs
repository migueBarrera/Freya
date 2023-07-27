using Freya.Desktop.Core.Features.Dialogs;
using FreyaManager.Features.Clients;

namespace FreyaManager.Framework;

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

    internal static IServiceCollection ConfigureHostAndAppServices(this IServiceCollection serviceProvider, Application application)
    {
        serviceProvider.Configure<HostOptions>(o => o.ShutdownTimeout = TimeSpan.FromSeconds(30));

        serviceProvider.ConfigureAppServices();
        serviceProvider.AddSingleton<IHostedService>(x => x.GetRequiredService<MainWindow>());
        serviceProvider.AddSingleton<MainWindow>();
        serviceProvider.AddSingleton(application);

        return serviceProvider;
    }

    internal static IServiceCollection ConfigureAppServices(this IServiceCollection serviceProvider)
    {
        ConfigureCommonServices(serviceProvider);
        ConfigureInternalServices(serviceProvider);
        ConfigureViewModel(serviceProvider);
        ConfigurePages(serviceProvider);
        InternalConfigureAppCenter(serviceProvider);
        serviceProvider.UseDesktopCoreDialogs<DialogWindowsService>();
        serviceProvider.UseDesktopUpdater();

        return serviceProvider;
    }

    private static void ConfigureCommonServices(IServiceCollection services)
    {
        services.AddSingleton<IAuthTokenService, AuthTokenService>();
        services.AddSingleton<ISessionService, SessionService>();
        services.AddTransient<IConnectivityService, ConnectivityService>();
        services.AddTransient<IAppSecretsService, AppSecretsService>();
        services.AddTransient<IRefitService, RefitService>();
        services.AddTransient<ITaskHelper, TaskHelper>();
        services.AddTransient<ITaskHelperFactory, TaskHelperFactory>();
        services.AddTransient<IEmployeeRolService, EmployeeRolService>();
        services.AddTransient<SubscriptionChargesService>();
        services.AddTransient<IEditClinicService, EditClinicService>();
        services.AddTransient<ClientItemViewModel>();
        services.AddTransient<MultimediaService>();
        services.AddTransient<MultimediaDialogViewModel>();
    }

    private static void ConfigureInternalServices(IServiceCollection services)
    {
        services.AddSingleton<ITranslatorService, TranslatorService>();
        services.AddSingleton<INavigationService, NavigationService>();
        services.AddTransient<EditClientService>();
        services.AddTransient<EditClientViewModel>();
        services.AddTransient<CompaniesService>();
        services.AddTransient<ISignInService, SignInService>();
        services.AddTransient<EmployeeService>();
        services.AddTransient<IClinicService, ClinicService>();
        services.AddSingleton<ICurrentAppManagerService, CurrentAppManagerService>();
        services.AddTransient<IClientDetailService, ClientDetailService>();
        services.AddTransient<SubscriptionService>();
        services.AddTransient<FaqService>();
        services.AddTransient<SettingsService>();

        services.AddTransient<ClientService>();
        services.AddTransient<CheckEmployeeService>();
    }
    
    private static void ConfigureViewModel(IServiceCollection services)
    {
        services.AddTransient<SignInViewModel>();
        services.AddTransient<CompaniesViewModel>();
        services.AddTransient<NewCompanyViewModel>();
        services.AddTransient<DetailCompanyViewModel>();
        services.AddTransient<CreateEmployeeViewModel>();
        services.AddTransient<CreateClinicViewModel>();
        services.AddTransient<ClinicDetailViewModel>();
        services.AddTransient<LateralMenuViewModel>();
        services.AddTransient<HeaderControlViewModel>();
        services.AddTransient<SubscriptionsViewModel>();
        services.AddTransient<SoundMultimediaViewModel>();
        services.AddTransient<ImageMultimediaViewModel>();
        services.AddTransient<VideoMultimediaViewModel>();
        services.AddTransient<SubscriptionProductItemViewModel>();
        services.AddTransient<SubscriptionPaymentViewModel>();
        services.AddTransient<FaqViewModel>();
        services.AddTransient<EditFaqViewModel>();
        services.AddTransient<NewFaqViewModel>();
        services.AddTransient<FaqItemViewModel>();
        services.AddTransient<NewSubscriptionViewModel>();
        services.AddTransient<SettingViewModel>();
        services.AddTransient<PaymentPlansForCompanyItemViewModel>();
        services.AddTransient<ClinicDetailEmployeesViewModel>();
        services.AddTransient<ClinicDetailClientsViewModel>();
        services.AddTransient<ClinicsForCompanyViewModel>();
        services.AddTransient<EmployeesForCompanyViewModel>();
        services.AddTransient<CheckEmployeeViewModel>();
        services.AddTransient<ChargesViewModel>();
        services.AddTransient<EditClinicViewModel>();
        services.AddTransient<EditCompanyViewModel>();
        services.AddTransient<ClientDetailViewModel>();
        services.AddSingleton<MainViewModel>();
    }
    
    private static void ConfigurePages(IServiceCollection services)
    {
        services.AddTransient<SignInPage>();
        services.AddTransient<CompaniesPage>();
        services.AddTransient<NewCompanyPage>();
        services.AddTransient<DetailCompanyPage>();
        services.AddTransient<CreateEmployeePage>();
        services.AddTransient<CreateClinicPage>();
        services.AddTransient<ClinicDetailPage>();
        services.AddTransient<SubscriptionsPage>();
        services.AddTransient<FaqPage>();
        services.AddTransient<EditFaqPage>();
        services.AddTransient<NewFaqPage>();
        services.AddTransient<NewSubscriptionPage>();
        services.AddTransient<SettingPage>();
        services.AddTransient<CheckEmployeePage>();
        services.AddTransient<ChargesPage>();
        services.AddTransient<EditClinicPage>();
        services.AddTransient<EditCompanyPage>();
        services.AddTransient<ClientDetailPage>();
        services.AddTransient<EditClientPage>();
    }
}
