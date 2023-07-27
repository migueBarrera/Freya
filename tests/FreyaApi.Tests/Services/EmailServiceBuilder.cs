namespace FreyaApi.Tests.Services;

public class EmailServiceBuilder
{
    public EmailService Build()
    {
        ILogger<EmailService> logger = new NullLogger<EmailService>();
        return new EmailService(logger);
    }
}
