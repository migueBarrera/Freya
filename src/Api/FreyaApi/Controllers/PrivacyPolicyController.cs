namespace FreyaApi.Controllers;

[Route("api/[controller]/v1")]
public class PrivacyPolicyController : Controller
{
    [HttpGet]
    [AllowAnonymous]
    public IActionResult Get()
    {
        var html = System.IO.File.ReadAllText("./Views/privacy-policy.html");

        return new ContentResult
        {
            ContentType = "text/html",
            Content = html
        };
    }
}
