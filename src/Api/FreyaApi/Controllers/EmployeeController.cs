namespace FreyaApi.Controllers;

[Authorize]
[Route("api/[controller]/v1")]
[ApiController]
public class EmployeeController : ControllerBase
{
    private readonly EmployeeControllerServices employeeControllerServices;

    public EmployeeController(
        EmployeeControllerServices employeeControllerServices)
    {
        this.employeeControllerServices = employeeControllerServices;
    }

    [HttpPost]
    [Authorize(Roles = $"{SystemRoles.APP_MANAGER} , {SystemRoles.COMPANY_MANAGER}, {SystemRoles.COMPANY_OWNER}, {SystemRoles.CLINIC_MANAGER}, {SystemRoles.CLINIC_OFFICER}")]
    [Route("Create")]
    public async Task<ActionResult<EmployeeSignUpResponse>> CreateEmployee(EmployeeSignUpRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ErrorModel(ErrorType.REQUIRED_PARAMETERS));
        }

        return await employeeControllerServices.CreateEmployee(this, request);
    }

    [HttpPost]
    [AllowAnonymous]
    [Route("SignIn")]
    public Task<ActionResult<EmployeeSignInResponse>> PostSignIn(EmployeeSignInRequest request)
    {
        return employeeControllerServices.PostSignIn(this, request);
    }

    [HttpPut]
    [Authorize(Roles = $"{SystemRoles.APP_MANAGER} , {SystemRoles.COMPANY_MANAGER}, {SystemRoles.COMPANY_OWNER}, {SystemRoles.CLINIC_MANAGER}, {SystemRoles.CLINIC_OFFICER}")]
    public Task<ActionResult> UpdateEmployee(EmployeeUpdateRequest request)
    {
        return employeeControllerServices.UpdateEmployee(this, request);
    }

    [HttpPost]
    [Route("changePass")]
    [Authorize(Roles = $"{SystemRoles.APP_MANAGER} , {SystemRoles.COMPANY_MANAGER}, {SystemRoles.COMPANY_OWNER}, {SystemRoles.CLINIC_MANAGER}, {SystemRoles.CLINIC_OFFICER}")]
    ////
    ///For Security, you only can change your password
    public Task<ActionResult> ChangePassword(EmployeeChangePassRequest request)
    {
        return employeeControllerServices.ChangePassword(this, request);
    }

    [HttpPost]
    [AllowAnonymous]
    [Route("recoverPass")]
    public Task<ActionResult> RecoverPassword([FromBody] EmployeeRecoverPassRequest request)
    {
        return employeeControllerServices.RecoverPassword(this, request.Email);
    }

    [HttpPost]
    [Route("checkStateForRegister")]
    [Authorize(Roles = $"{SystemRoles.APP_MANAGER} , {SystemRoles.COMPANY_MANAGER}, {SystemRoles.COMPANY_OWNER}, {SystemRoles.CLINIC_OFFICER}, {SystemRoles.CLINIC_MANAGER}")]
    public Task<ActionResult<CheckEmployeeStateForRegisterResponse>> CheckEmployeeStateForRegister(CheckEmployeeStateForRegisterResquest request)
    {
        return employeeControllerServices.CheckEmployeeStateForRegister(this, request);
    }
    
    [HttpPost]
    [Route("checkStateForRegisterForCompany")]
    [Authorize(Roles = $"{SystemRoles.APP_MANAGER}")]
    public Task<ActionResult<CheckEmployeeStateForRegisterResponse>> CheckEmployeeStateForRegisterForCompany(CheckEmployeeStateForRegisterForCompanyResquest request)
    {
        return employeeControllerServices.CheckEmployeeStateForRegisterForCompany(this, request);
    }

    [HttpPost]
    [Route("assignToClinic")]
    [Authorize(Roles = $"{SystemRoles.APP_MANAGER} , {SystemRoles.COMPANY_MANAGER}, {SystemRoles.COMPANY_OWNER}, {SystemRoles.CLINIC_OFFICER}, {SystemRoles.CLINIC_MANAGER}")]
    public async Task<IActionResult> AssignToClinic(AssignEmployeeToClinicRequest request)
    {
        return await employeeControllerServices.AssignToClinic(this, request);
    }

    [HttpPost]
    [Route("unassignToClinic")]
    [Authorize(Roles = $"{SystemRoles.APP_MANAGER} , {SystemRoles.COMPANY_MANAGER}, {SystemRoles.COMPANY_OWNER}, {SystemRoles.CLINIC_OFFICER}, {SystemRoles.CLINIC_MANAGER}")]
    public async Task<IActionResult> UnAssignToClinic(UnassignEmployeeToClinicRequest request)
    {
        return await employeeControllerServices.UnAssignToClinic(this, request);
    }

    [HttpGet]
    [Route("{id}/getClinics")]
    [Authorize(Roles = $"{SystemRoles.APP_MANAGER} , {SystemRoles.COMPANY_MANAGER}, {SystemRoles.COMPANY_OWNER}, {SystemRoles.CLINIC_OFFICER}, {SystemRoles.CLINIC_MANAGER}")]
    public async Task<ActionResult<IEnumerable<ClinicResponse>>> GetClinicsFromEmployeeByRol(Guid id)
    {
        return await employeeControllerServices.GetClinicsFromEmployeeByRol(this, id);
    }
}
