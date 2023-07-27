namespace Freya.Services;

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
        return configuration.GetRequiredSection("AppCenter:Secret")?.Value ?? string.Empty;
    }
}
