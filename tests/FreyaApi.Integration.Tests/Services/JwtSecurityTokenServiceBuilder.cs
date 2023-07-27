using MiBebeApi.Services;

namespace FreyaApi.Integration.Tests.Services;

public class JwtSecurityTokenServiceBuilder
{
    public JwtSecurityTokenService Build()
    {
        var configuration = new ConfigurationBuilder().Build();

        return new JwtSecurityTokenService(configuration);
    }
}
