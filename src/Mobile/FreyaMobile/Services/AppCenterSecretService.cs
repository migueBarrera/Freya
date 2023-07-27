namespace FreyaMobile.Services;

internal class AppCenterSecretService : IAppCenterSecretService
{
    private readonly IConfiguration configuration;

    public AppCenterSecretService(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public bool IsEnabledAnalitics => configuration.GetValue<bool>("AppCenter:IsEnabledAnalitics");

    public bool IsEnabledCrashes => configuration.GetValue<bool>("AppCenter:IsEnabledCrashes");

    public string GetSecret()
    {
        //TODO IOS
        var appCenterSecret = configuration.GetRequiredSection("AppCenter:SecretAndroid").Value;

        return appCenterSecret ?? string.Empty;
    }
}
