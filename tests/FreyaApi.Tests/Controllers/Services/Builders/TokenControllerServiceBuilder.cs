namespace FreyaApi.Tests.Controllers.Services.Builders;

public class TokenControllerServiceBuilder
{
    private JwtSecurityTokenService jwtSecurityTokenService;

    public TokenControllerServiceBuilder()
    {
        jwtSecurityTokenService = new JwtSecurityTokenServiceBuilder().Build();
    }

    public TokenControllerService Build()
    {
        return new TokenControllerService(jwtSecurityTokenService);
    }
}
