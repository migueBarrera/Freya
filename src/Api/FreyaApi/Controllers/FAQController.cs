namespace FreyaApi.Controllers;

[Authorize]
[Route("api/[controller]/v1")]
[ApiController]
public class FAQController : Controller
{
    private FAQControllerService fAQControllerService;

    public FAQController(FAQControllerService fAQControllerService)
    {
        this.fAQControllerService = fAQControllerService;
    }

    [HttpGet]
    [Authorize(Roles = $"{SystemRoles.APP_MANAGER} , {SystemRoles.COMPANY_MANAGER}, {SystemRoles.COMPANY_OWNER}, {SystemRoles.CLINIC_MANAGER}, {SystemRoles.CLINIC_OFFICER}")]
    public async Task<ActionResult<IEnumerable<FAQ>>> GetFAQs()
    {
        return await fAQControllerService.GetFAQs(this);

    }

    [HttpPost]
    [Authorize(Roles = SystemRoles.APP_MANAGER)]
    public async Task<IActionResult> Create(FAQ request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        return await fAQControllerService.Create(this, request);
    }

    [HttpDelete]
    [Authorize(Roles = SystemRoles.APP_MANAGER)]
    [Route("{faqId}")]
    public async Task<IActionResult> DeleteFaq(Guid faqId)
    {
        return await fAQControllerService.DeleteFaq(this, faqId);
    }

    [HttpPut]
    [Authorize(Roles = SystemRoles.APP_MANAGER)]
    public async Task<IActionResult> UpdateFaq(FAQ request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        return await fAQControllerService.UpdateFaq(this, request);
    }
}
