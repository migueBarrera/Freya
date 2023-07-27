using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreyaApi.Integration.Tests;

public class ConfigurationBuilder
{
    public IConfiguration Build()
    {
        var configuration = new Mock<IConfiguration>();

        var configurationSection = new Mock<IConfigurationSection>();
        configurationSection.Setup(x => x.Value).Returns(Consts.Test_Key);
        configuration.Setup(x => x.GetSection(It.Is<string>(k => k == "Jwt:Key"))).Returns(configurationSection.Object);

        return configuration.Object;
    }
}
