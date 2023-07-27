namespace FreyaManager.Framework;

internal class AppSecretsService : IAppSecretsService
{
    private readonly IConfiguration configuration;

    public AppSecretsService(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public string GetUrlBase()
    {
        return configuration.GetValue<string>("ServerUri") ?? string.Empty;
    }
}