namespace FreyaApi.Controllers;

[Authorize]
[Route("api/[controller]/v1")]
[ApiController]
public class AppManagerController : ControllerBase
{
    private readonly AppManagerControllerService appManagerControllerService;

    public AppManagerController(
        AppManagerControllerService appManagerControllerService)
    {
        this.appManagerControllerService = appManagerControllerService;
    }

    [AllowAnonymous]
    [HttpPost]
    [Route("SignIn")]
    public ActionResult<AppManagerSignInResponse> PostSignIn(AppManagerSignInRequest request)
    {
        return appManagerControllerService.PostSignIn(this, request);
    }


}
