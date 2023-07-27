namespace FreyaApi.Controllers;

[Route("api/[controller]/v1")]
[ApiController]
public class TokenController : Controller
{
    private readonly TokenControllerService tokenControllerService;

    public TokenController(TokenControllerService tokenControllerService)
    {
        this.tokenControllerService = tokenControllerService;
    }

    [HttpPost]
    [AllowAnonymous]
    [Route("refresh_token")]
    public ActionResult<RefreshTokenResponse> RefresTokenClient(RefreshTokenRequest request)
    {
        return tokenControllerService.RefresTokenClient(this, request);
    }
}
