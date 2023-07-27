namespace ApiContract.Refit;

public class RefitService : IRefitService
{
    private IAppSecretsService appSecretsService;
    private IAuthTokenService authTokenService;

    public RefitService(
        IAppSecretsService appSecretsService, 
        IAuthTokenService authTokenService)
    {
        this.appSecretsService = appSecretsService;
        this.authTokenService = authTokenService;
    }

    public T InitRefitInstance<T>(bool isAutenticated = false)
    {
        var handler = WireHttpHandlers();

        var httpClient = GetClient(handler, isAutenticated);

        return RestService.For<T>(httpClient);
    }

    private HttpClient GetClient(DelegatingHandler? handler, bool isAutenticated)
    {
        HttpClient? client = null;

        if (isAutenticated)
        {
            client = new HttpClient(
                       new AuthenticatedParameterizedHttpClientHandler(
                               authTokenService,
                               handler));
        }
        else if (handler != null)
        {
            client = new HttpClient(handler);
        }
        else
        {
            client = new HttpClient();
        }

        client.BaseAddress = new Uri(appSecretsService.GetUrlBase());
        client.Timeout = TimeSpan.FromSeconds(60);

        return client;
    }

    private DelegatingHandler? WireHttpHandlers()
    {
        DelegatingHandler? handler = null;

#if DEBUG
        handler = new HttpLoggingHandler();
#endif

        return handler;
    }
}
