using System.Diagnostics.CodeAnalysis;

namespace FreyaApi.Tests.Services;

[TestFixture]
public class EmailServiceShould
{
    private EmailServiceBuilder _emailServiceBuilder;

    public EmailServiceShould()
    {
        _emailServiceBuilder = new EmailServiceBuilder();
    }

    [Test]
    [Ignore("test only for development propouses")]
    [ExcludeFromCodeCoverage]
    public void SendTestEmailWelcomeEmployee()
    {
        var service = _emailServiceBuilder.Build();

        var resultEmail = service.SendMailWelcomeEmployee("mbmdevelop@gmail.com", "test", "contraseña");

        Assert.That(resultEmail, Is.True);
    }
    
    [Test]
    [Ignore("test only for development propouses")]
    [ExcludeFromCodeCoverage]
    public void SendTestEmailWelcomeClient()
    {
        var service = _emailServiceBuilder.Build();

        var resultEmail = service.SendMailWelcomeClient("mbmdevelop@gmail.com", "Miguel", "123456");

        Assert.That(resultEmail, Is.True);
    }
    
    [Test]
    [Ignore("test only for development propouses")]
    [ExcludeFromCodeCoverage]
    public void SendMailChangePassEmployee()
    {
        var service = _emailServiceBuilder.Build();

        var resultEmail = service.SendMailChangePassEmployee("mbmdevelop@gmail.com", "123456");

        Assert.That(resultEmail, Is.True);
    }

    [Test]
    [Ignore("test only for development propouses")]
    [ExcludeFromCodeCoverage]
    public void SendTestEmailChangePassClient()
    {
        var service = _emailServiceBuilder.Build();

        var resultEmail = service.SendMailChangePassClient("mbmdevelop@gmail.com", "123456");

        Assert.That(resultEmail, Is.True);
    }
}
