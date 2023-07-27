using Microsoft.AspNetCore.Html;

namespace FreyaApi.Services;

public class EmailService : IEmailService
{
    private readonly ILogger<EmailService> _logger;
    private readonly string from = string.Empty;
    private readonly string fromPass = string.Empty;

    public EmailService(ILogger<EmailService> logger)
    {
        _logger = logger;

        from = "freya.soft.app@gmail.com";
        fromPass = "pdfkukdoljaocwtt";
    }

    public bool SendMailChangePassEmployee(string to, string pass)
    {
        string pathToHtmlFile = @"./Views/passchanged-employee.html";
        string body = System.IO.File.ReadAllText(pathToHtmlFile);

        // Replace [NAME] placeholder with actual name
        body = body.Replace("[PASS]", pass);

        return SendMail(to, "Contraseña cambiada", body);
    }
    
    public bool SendMailChangePassClient(string to, string pass)
    {
        string pathToHtmlFile = @"./Views/passchanged-client.html";
        string body = System.IO.File.ReadAllText(pathToHtmlFile);

        // Replace [NAME] placeholder with actual name
        body = body.Replace("[PASS]", pass);

        return SendMail(to, "Contraseña cambiada", body);
    }
    
    public bool SendMailWelcomeClient(string to, string name, string pass)
    {
        string pathToHtmlFile = @"./Views/welcomeclient.html";
        string body = System.IO.File.ReadAllText(pathToHtmlFile);

        // Replace [NAME] placeholder with actual name
        body = body.Replace("[NAME]", name);
        body = body.Replace("[PASS]", pass);

        return SendMail(to, "Bienvenido", body);
    }
    
    public bool SendMailWelcomeEmployee(string to, string name, string pass)
    {
        string pathToHtmlFile = @"./Views/welcomeemployee.html";
        string body = System.IO.File.ReadAllText(pathToHtmlFile);

        // Replace [NAME] placeholder with actual name
        body = body.Replace("[NAME]", name);
        body = body.Replace("[PASS]", pass);

        return SendMail(to, "Bienvenido", body);
    }

    private bool SendMail(string to, string subject, string body)
    {
        var result = false;
        MailMessage message = new MailMessage(from, to);

       

        message.Subject = subject;
        message.Body = body;
        message.BodyEncoding = Encoding.UTF8;
        message.IsBodyHtml = true;
        SmtpClient client = new SmtpClient("smtp.gmail.com", 587); //Gmail smtp    
        System.Net.NetworkCredential basicCredential1 = new
        System.Net.NetworkCredential(from, fromPass);
        client.EnableSsl = true;
        client.UseDefaultCredentials = false;
        client.Credentials = basicCredential1;
        try
        {
            client.Send(message);
            result = true;
        }

        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
        }

        return result;
    }

    private bool SendMail(IEnumerable<string> to, string subject, string body)
    {
        foreach (var item in to)
        {
            SendMail(item, subject, body);
        }

        return true;
    }
}
