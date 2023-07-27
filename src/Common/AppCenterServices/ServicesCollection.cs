namespace AppCenterServices;

public static class ServicesCollection
{
    public static void UseAppCenter<AppCenterLogImplementation, AppCenterSecretsImplementation>(this IServiceCollection serviceProvider) 
        where AppCenterLogImplementation : class, IAppCenterLogService
        where AppCenterSecretsImplementation : class, IAppCenterSecretService
    {
        serviceProvider.AddTransient<IAppCenterLogService, AppCenterLogImplementation>();
        serviceProvider.AddTransient<IAppCenterSecretService, AppCenterSecretsImplementation>();
        serviceProvider.AddTransient<AppCenterService>();
        serviceProvider.AddTransient<AppCenterAnalitics>();
    }
}
