using Microsoft.Extensions.Configuration;

namespace FreyaApi.Tests.Services;

public class JwtSecurityTokenServiceBuilder
{
    private Mock<IConfiguration> configuration;

    public JwtSecurityTokenServiceBuilder()
    {
        configuration = new Mock<IConfiguration>();

        var configurationSection = new Mock<IConfigurationSection>();
        configurationSection.Setup(x => x.Value).Returns(Consts.Test_Key);
        configuration.Setup(x => x.GetSection(It.Is<string>(k => k == "Jwt:Key"))).Returns(configurationSection.Object);
    }

    public JwtSecurityTokenService Build()
    {
        return new JwtSecurityTokenService(configuration.Object);
    }
}
