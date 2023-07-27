namespace FreyaApi.Controllers;

//[Authorize]
[Route("api/[controller]/v1")]
[ApiController]
public class HelperController : ControllerBase
{
    private readonly HelperControllerService helperControllerService;

    public HelperController(HelperControllerService helperControllerService)
    {
        this.helperControllerService = helperControllerService;
    }

    [Route("reset")]
    [HttpGet]
    public async Task<IActionResult> ResetDatabase([FromQuery] bool delete)
    {
        return await helperControllerService.RecreateDatabase(this, delete);
    }

    [HttpGet]
    [Route("info")]
    public ActionResult<ApiInfo> Info()
    {
        return helperControllerService.Info(this);
    }
}
