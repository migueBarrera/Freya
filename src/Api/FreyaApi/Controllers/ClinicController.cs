namespace FreyaApi.Controllers;

[Authorize]
[Route("api/[controller]/v1")]
[ApiController]
public class ClinicController : ControllerBase
{
    private readonly ClinicControllerService clinicControllerService;

    public ClinicController(
        ClinicControllerService clinicControllerService)
    {
        this.clinicControllerService = clinicControllerService;
    }

    [HttpPost]
    [Authorize(Roles = $"{SystemRoles.APP_MANAGER} , {SystemRoles.COMPANY_MANAGER}, {SystemRoles.COMPANY_OWNER}")]
    public async Task<IActionResult> Create([FromBody] ClinicCreateRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        return await clinicControllerService.Create(this, request);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = $"{SystemRoles.APP_MANAGER} , {SystemRoles.COMPANY_MANAGER}, {SystemRoles.COMPANY_OWNER}, {SystemRoles.CLINIC_OFFICER}, {SystemRoles.CLINIC_MANAGER}")]
    public async Task<ActionResult<ClinicDetailResponse>> Details(Guid id)
    {
        return await clinicControllerService.Details(this, id);
    }

    [HttpGet("{id}/employees")]
    [Authorize(Roles = $"{SystemRoles.APP_MANAGER} , {SystemRoles.COMPANY_MANAGER}, {SystemRoles.COMPANY_OWNER}, {SystemRoles.CLINIC_OFFICER}, {SystemRoles.CLINIC_MANAGER}")]
    public ActionResult<PagedModel<EmployeeResponse>> GetEmployees(Guid id, [FromQuery] PaginationFilter paginationFilter)
    {
        return clinicControllerService.GetEmployees(this, id, paginationFilter);

    }

    [HttpPut]
    [Authorize(Roles = $"{SystemRoles.APP_MANAGER} , {SystemRoles.COMPANY_MANAGER}, {SystemRoles.COMPANY_OWNER}, {SystemRoles.CLINIC_MANAGER}")]
    public async Task<ActionResult> UpdateClinic(UpdateClinicRequest request)
    {
        return await clinicControllerService.UpdateClinic(this, request);
    }

    [HttpGet("{id}/clients")]
    [Authorize(Roles = $"{SystemRoles.APP_MANAGER} , {SystemRoles.COMPANY_MANAGER}, {SystemRoles.COMPANY_OWNER}, {SystemRoles.CLINIC_OFFICER}, {SystemRoles.CLINIC_MANAGER}")]
    public ActionResult<PagedModel<ClientResponse>> GetClientsForClinic(Guid id, [FromQuery] PaginationFilter paginationFilter)
    {
        return clinicControllerService.GetClientsForClinic(this, id, paginationFilter);
    }

    [HttpDelete]
    [Authorize(Roles = $"{SystemRoles.APP_MANAGER} , {SystemRoles.COMPANY_MANAGER}, {SystemRoles.COMPANY_OWNER}, {SystemRoles.CLINIC_OFFICER}, {SystemRoles.CLINIC_MANAGER}")]
    [Route("{clinicId}")]
    public async Task<IActionResult> DeleteClinic(Guid clinicId)
    {
        return await clinicControllerService.DeleteClinic(this, clinicId);
    }
}
