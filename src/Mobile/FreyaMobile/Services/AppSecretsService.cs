namespace FreyaMobile.Services;

internal class AppSecretsService : IAppSecretsService
{
    private readonly IConfiguration configuration;

    public AppSecretsService(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public string GetUrlBase()
    {
        return configuration.GetRequiredSection("Server:BaseUri").Value ?? string.Empty;
    }
}
