namespace FreyaApi.Services.Interfaces;

public interface IEmailService
{
    public bool SendMailChangePassEmployee(string to, string pass);

    public bool SendMailChangePassClient(string to, string pass);

    public bool SendMailWelcomeClient(string to, string name, string pass);

    public bool SendMailWelcomeEmployee(string to, string name, string pass);
}
