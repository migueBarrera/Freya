using FreyaApi.Controllers.Services;

namespace FreyaApi.Controllers;

[Authorize]
[Route("api/[controller]/v1")]
[ApiController]
public class CompaniesController : Controller
{
    private readonly CompaniesControllerService companiesControllerService;

    public CompaniesController(
        CompaniesControllerService companiesControllerService)
    {
        this.companiesControllerService = companiesControllerService;
    }

    // GET: Companies
    [HttpGet]
    [Authorize(Roles = SystemRoles.APP_MANAGER)]
    public ActionResult<PagedModel<Company>> Index([FromQuery] PaginationFilter paginationFilter)
    {
        return companiesControllerService.Index(this, paginationFilter);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = SystemRoles.APP_MANAGER)]
    public async Task<ActionResult<CompanyDetailResponse>> Details(Guid id)
    {
        return await companiesControllerService.Details(this, id);
    }

    [HttpGet("{id}/clinics")]
    [Authorize(Roles = $"{SystemRoles.APP_MANAGER} , {SystemRoles.COMPANY_MANAGER}, {SystemRoles.COMPANY_OWNER}")]
    public ActionResult<PagedModel<ClinicResponse>> GetClinicsByCompanyId(Guid id, [FromQuery] PaginationFilter paginationFilter)
    {
        return companiesControllerService.GetClinicsByCompanyId(this, id, paginationFilter);
    }

    [HttpGet("{id}/employees")]
    [Authorize(Roles = $"{SystemRoles.APP_MANAGER} , {SystemRoles.COMPANY_MANAGER}, {SystemRoles.COMPANY_OWNER}")]
    public ActionResult<PagedModel<EmployeeResponse>> GetEmployeesByCompanyId(Guid id, [FromQuery] PaginationFilter paginationFilter)
    {
        return companiesControllerService.GetEmployeesByCompanyId(this, id, paginationFilter);
    }

    [HttpPost]
    [Authorize(Roles = SystemRoles.APP_MANAGER)]
    public async Task<IActionResult> Create(CompanyCreateRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        return await companiesControllerService.Create(this, request);
    }

    [HttpPut]
    [Authorize(Roles = $"{SystemRoles.APP_MANAGER}")]
    public async Task<ActionResult> UpdateCOmpany(UpdateCommpanyRequest request)
    {
        return await companiesControllerService.UpdateCcompany(this, request);
    }

    [HttpDelete]
    [Authorize(Roles = $"{SystemRoles.APP_MANAGER}")]
    [Route("{companyId}")]
    public async Task<IActionResult> DeleteClinic(Guid companyId)
    {
        return await companiesControllerService.DeleteCompany(this, companyId);
    }
}
