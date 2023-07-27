namespace Freya.Desktop.Core.Services;

public class AppSecretsService : IAppSecretsService
{
    private readonly IConfiguration configuration;

    public AppSecretsService(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public string GetUrlBase()
    {
        return configuration.GetValue<string>("Server:BaseUri") ?? string.Empty;
    }
}