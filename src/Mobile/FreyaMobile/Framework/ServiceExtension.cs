namespace FreyaMobile.Framework;

internal static class ServiceExtension
{
    internal static MauiAppBuilder ConfigureAppConfiguration(this MauiAppBuilder builder)
    {
        var a = Assembly.GetExecutingAssembly();
        using var stream = a.GetManifestResourceStream("FreyaMobile.app.settings.json") 
            ?? throw new ArgumentNullException($"Stream can not be null on {nameof(ConfigureAppConfiguration)}");

        var config = new ConfigurationBuilder()
            .AddJsonStream(stream)
            .Build();

        builder.Configuration.AddConfiguration(config);

        return builder;
    }

    internal static MauiAppBuilder ConfigureNativeServices(this MauiAppBuilder builder)
    {
        //TODO IOS
#if ANDROID
        builder.Services.AddTransient<INativeMediaService, Platforms.NativeMediaService>();
#endif

        return builder;
    }
    
    internal static MauiAppBuilder ConfigureApiContractServices(this MauiAppBuilder builder)
    {
        builder.Services.AddTransient<IRefitService, RefitService>();
        builder.Services.AddTransient<IAppSecretsService, AppSecretsService>();
        builder.Services.AddTransient<IAuthTokenService, AuthTokenService>();

        return builder;
    }

    internal static MauiAppBuilder ConfigureServices(this MauiAppBuilder builder)
    {
        builder.Services.AddTransient(typeof(ILogger<>), typeof(Logger<>));
        builder.Services.AddTransient<ICurrentUserService, CurrentUserService>();
        builder.Services.AddTransient<IChangePassService, ChangePassService>();
        builder.Services.AddTransient<IForgotPassService, ForgotPassService>();
        builder.Services.AddTransient<IRegisterClientDataService, RegisterClientDataService>();
        builder.Services.AddTransient<IRegisterClientService, RegisterClientService>();
        builder.Services.AddTransient<ITranslatorService, TranslatorService>();

        builder.Services.AddTransient<IAuthenticationService, AuthenticationService>();

        //HelpAndInfo
        //builder.Services.AddTransient<IMenuHelpAndInfoService, MenuHelpAndInfoService>();

        //Client
        builder.Services.AddTransient<IClientDataService, ClientDataService>();
        builder.Services.AddTransient<IUserService, UserService>();
        builder.Services.AddTransient<IEditClientService, EditClientService>();

        //Clinics
        builder.Services.AddTransient<MenuClinicService>();

        //Images
        builder.Services.AddTransient<IImagesService, ImagesService>();
        
        //Videos
        builder.Services.AddTransient<IVideosService, VideosService>();
        
        //Sounds
        builder.Services.AddTransient<SoundsService>();

        //Fertile Days
        //builder.Services.AddTransient<IFertileDaysService, FertileDaysService>();

        return builder;
    }

    internal static MauiAppBuilder ConfigureViewModels(this MauiAppBuilder builder)
    {
        //HelpAndInfo
        //builder.Services.AddTransient<HelpAndInfoViewModel>();

        //Client
        builder.Services.AddTransient<UserNotLoggedViewModel>();
        builder.Services.AddTransient<UserLoggedViewModel>();
        builder.Services.AddTransient<ClientViewModel>();
        builder.Services.AddTransient<EditClientViewModel>();
        builder.Services.AddTransient<ChangePassViewModel>();
        builder.Services.AddTransient<ForgotPassViewModel>();
        builder.Services.AddTransient<RegisterViewModel>();
        
        //Clinics
        builder.Services.AddTransient<ClinicsViewModel>();
        builder.Services.AddTransient<ClinicNotLoggedViewModel>();
        builder.Services.AddTransient<ClinicLoggedWithOutClinicDataViewModel>();
        builder.Services.AddTransient<ClinicLoggedWithClinicDataViewModel>();

        //Images
        builder.Services.AddTransient<ImagesViewModel>();
        builder.Services.AddTransient<DetailImageViewModel>();

        //Videos
        builder.Services.AddTransient<VideosViewModel>();
        builder.Services.AddTransient<DetailVideoViewModel>();

        //Sounds
        builder.Services.AddTransient<SoundsViewModel>();
        builder.Services.AddTransient<DetailSoundViewModel>();

        //Fertile Days
        //builder.Services.AddTransient<FertileDayViewModel>();
        //builder.Services.AddTransient<FertileDayDetailViewModel>();

        

        return builder;
    }

    internal static MauiAppBuilder ConfigurePages(this MauiAppBuilder builder)
    {
        builder.Services.AddTransient<YesNoDialog>();

        //HelpAndInfo
        //builder.Services.AddTransient<HelpAndInfoPage>();

        //Client
        builder.Services.AddTransient<ClientPage>();
        builder.Services.AddTransient<EditClientPage>();
        builder.Services.AddTransient<ChangePassPage>();
        builder.Services.AddTransient<ForgotPassPage>();
        builder.Services.AddTransient<RegisterPage>();

        //Clinics
        builder.Services.AddTransient<ClinicsPage>();

        //Images
        builder.Services.AddTransient<ImagesPage>();
        builder.Services.AddTransient<DetailImagePage>();
        
        //Videos
        builder.Services.AddTransient<VideosPage>();
        builder.Services.AddTransient<DetailVideoPage>();
        
        //Sounds
        builder.Services.AddTransient<SoundsPage>();
        builder.Services.AddTransient<DetailSoundPage>();
        
        //Fertile Days
        //builder.Services.AddTransient<FertileDayPage>();
        //builder.Services.AddTransient<FertileDayDetailPage>();

        return builder;
    }

    internal static MauiAppBuilder RegisterShellRoutes(this MauiAppBuilder builder)
    {
        Routing.RegisterRoute(nameof(ClientPage), typeof(ClientPage));
        Routing.RegisterRoute(nameof(ChangePassPage), typeof(ChangePassPage));
        Routing.RegisterRoute(nameof(EditClientPage), typeof(EditClientPage));
        Routing.RegisterRoute(nameof(ForgotPassPage), typeof(ForgotPassPage));
        Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));
        Routing.RegisterRoute(nameof(ImagesPage), typeof(ImagesPage));
        Routing.RegisterRoute(nameof(VideosPage), typeof(VideosPage));
        Routing.RegisterRoute(nameof(SoundsPage), typeof(SoundsPage));
        Routing.RegisterRoute(nameof(DetailImagePage), typeof(DetailImagePage));
        Routing.RegisterRoute(nameof(DetailVideoPage), typeof(DetailVideoPage));
        Routing.RegisterRoute(nameof(DetailSoundPage), typeof(DetailSoundPage));
        //Routing.RegisterRoute(nameof(FertileDayPage), typeof(FertileDayPage));
        //Routing.RegisterRoute(nameof(FertileDayDetailPage), typeof(FertileDayDetailPage));

        return builder;
    }
}
