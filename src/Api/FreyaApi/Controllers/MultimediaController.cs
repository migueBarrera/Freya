namespace FreyaApi.Controllers;

[Authorize]
[Route("api/[controller]/v1")]
[ApiController]
public class MultimediaController : ControllerBase
{
    private readonly MultimediaControllerService multimediaControllerService;

    public MultimediaController(MultimediaControllerService multimediaControllerService)
    {
        this.multimediaControllerService = multimediaControllerService;
    }

    [HttpDelete]
    [Route("{id}")]
    [Authorize(Roles = $"{SystemRoles.APP_MANAGER} , {SystemRoles.COMPANY_MANAGER}, {SystemRoles.COMPANY_OWNER}, {SystemRoles.CLINIC_MANAGER}, {SystemRoles.CLINIC_OFFICER}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        return await multimediaControllerService.Delete(this, id);
    }

    [HttpPost]
    [Authorize(Roles = $"{SystemRoles.APP_MANAGER} , {SystemRoles.COMPANY_MANAGER}, {SystemRoles.COMPANY_OWNER}, {SystemRoles.CLINIC_MANAGER}, {SystemRoles.CLINIC_OFFICER}")]
    public async Task<ActionResult<Guid>> AssingMultimediaToClient([FromBody] CreateMultimediaRequest request)
    {
        return await multimediaControllerService.AssingMultimediaToClient(this, request);
    }
}
