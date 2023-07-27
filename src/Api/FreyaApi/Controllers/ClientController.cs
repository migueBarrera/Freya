namespace FreyaApi.Controllers;

[Authorize]
[Route("api/[controller]/v1")]
[ApiController]
public class ClientController : ControllerBase
{
    private readonly ClientControllerService clientControllerService;

    public ClientController(
        ClientControllerService clientControllerService)
    {
        this.clientControllerService = clientControllerService;
    }

    [HttpGet("{id}/clinic/{clinicId}")]
    [Authorize(Roles = $"{SystemRoles.APP_MANAGER} , {SystemRoles.COMPANY_MANAGER}, {SystemRoles.COMPANY_OWNER}, {SystemRoles.CLINIC_OFFICER}, {SystemRoles.CLINIC_MANAGER}")]
    public async Task<ActionResult<ClientDetailResponse>> ClientDetailsForAClinic(Guid id, Guid clinicId)
    {
        return await clientControllerService.ClientDetailsForAClinic(this, id, clinicId);
    }

    [HttpPost]
    [Authorize(Roles = $"{SystemRoles.APP_MANAGER} , {SystemRoles.COMPANY_MANAGER}, {SystemRoles.COMPANY_OWNER}, {SystemRoles.CLINIC_OFFICER}, {SystemRoles.CLINIC_MANAGER}")]
    [Route("SignUpForClinic")]
    public async Task<ActionResult<ClientSignUpResponse>> PostSignUpForClinic(ClientSignUpRequestForClinic request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ErrorModel(ErrorType.REQUIRED_PARAMETERS));
        }

        return await clientControllerService.SignUpForClinic(this, request);
    }

    [HttpPost]
    [AllowAnonymous]
    [Route("SignUp")]
    public async Task<ActionResult<ClientSignUpResponse>> PostSignUp(ClientSignUpRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ErrorModel(ErrorType.REQUIRED_PARAMETERS));
        }

        return await clientControllerService.SignUp(this, request);
    }

    [HttpPost]
    [AllowAnonymous]
    [Route("SignIn")]
    public Task<ActionResult<ClientSignInResponse>> PostSignIn(ClientSignInRequest request)
    {
        return clientControllerService.SignIn(this, request);
    }

    [HttpPost]
    [Authorize(Roles = $"{SystemRoles.CLIENT_MOBILE}")]
    [Route("{id}/refresh")]
    public Task<ActionResult<ClientSignInResponse>> RefresClient(Guid id)
    {
        return clientControllerService.RefresClient(this, id);
    }

    [HttpPost]
    [Authorize(Roles = $"{SystemRoles.COMPANY_MANAGER}, {SystemRoles.COMPANY_OWNER}, {SystemRoles.CLINIC_OFFICER}, {SystemRoles.CLINIC_MANAGER}")]
    [Route("updatePlan")]
    public Task<ActionResult> UpdatePlan(ClientUpdatePlanRequest request)
    {
        return clientControllerService.UpdatePlan(this, request);
    }

    [HttpPost]
    [Authorize(Roles = $"{SystemRoles.CLIENT_MOBILE}")]
    [Route("change_password")]
    public Task<ActionResult> ChangePass(ClientChangePassRequest request)
    {
        return clientControllerService.ChangePass(this, request);
    }

    [HttpPatch]
    [Authorize(Roles = $"{SystemRoles.APP_MANAGER} , {SystemRoles.CLIENT_MOBILE} , {SystemRoles.COMPANY_MANAGER}, {SystemRoles.COMPANY_OWNER}, {SystemRoles.CLINIC_OFFICER}, {SystemRoles.CLINIC_MANAGER}")]
    [Route("{id}")]
    public Task<ActionResult> UpdateClient(Guid id, ClientUpdateRequest request)
    {
        return clientControllerService.UpdateClient(this, id, request);
    }

    [HttpPost]
    [Authorize(Roles = $"{SystemRoles.APP_MANAGER} , {SystemRoles.COMPANY_MANAGER}, {SystemRoles.COMPANY_OWNER}, {SystemRoles.CLINIC_OFFICER}, {SystemRoles.CLINIC_MANAGER}")]
    [Route("checkStateForRegister")]
    public ActionResult<CheckClientStateForRegisterResponse> CheckClientStateForRegister(CheckClientStateForRegisterResquest request)
    {
        return clientControllerService.CheckClientStateForRegister(this, request);
    }

    [HttpPost]
    [Authorize(Roles = $"{SystemRoles.APP_MANAGER} , {SystemRoles.COMPANY_MANAGER}, {SystemRoles.COMPANY_OWNER}, {SystemRoles.CLINIC_OFFICER}, {SystemRoles.CLINIC_MANAGER}")]
    [Route("assignToClinic")]
    public Task<IActionResult> AssignToClinic(AssignClientToClinicRequest request)
    {
        return clientControllerService.AssignToClinic(this, request);
    }

    [HttpDelete]
    [Authorize(Roles = $"{SystemRoles.APP_MANAGER} , {SystemRoles.COMPANY_MANAGER}, {SystemRoles.COMPANY_OWNER}, {SystemRoles.CLINIC_OFFICER}, {SystemRoles.CLINIC_MANAGER}")]
    [Route("{clientId}/clinic/{clinicId}")]
    public Task<IActionResult> RemoveClientFromClinic(Guid clientId, Guid clinicId)
    {
        return clientControllerService.RemoveClientFromClinic(this, clientId, clinicId);
    }

    [HttpPost]
    [AllowAnonymous]
    [Route("recoverPass")]
    public Task<ActionResult> RecoverPassword([FromBody] ClientForgotPassRequest request)
    {
        return clientControllerService.RecoverPassword(this, request);
    }

    [HttpGet("{id}/ultrasound/{clinicId}")]
    [Authorize(Roles = $"{SystemRoles.APP_MANAGER} , {SystemRoles.COMPANY_MANAGER}, {SystemRoles.COMPANY_OWNER}, {SystemRoles.CLINIC_OFFICER}, {SystemRoles.CLINIC_MANAGER}, {SystemRoles.CLIENT_MOBILE}")]
    public Task<IActionResult> GetUltrasounds(Guid id, Guid clinicId)
    {
        return clientControllerService.GetUltrasounds(this, id, clinicId);
    }

    [HttpGet("{id}/videos/{clinicId}")]
    [Authorize(Roles = $"{SystemRoles.APP_MANAGER} , {SystemRoles.COMPANY_MANAGER}, {SystemRoles.COMPANY_OWNER}, {SystemRoles.CLINIC_OFFICER}, {SystemRoles.CLINIC_MANAGER}, {SystemRoles.CLIENT_MOBILE}")]
    public Task<IActionResult> GetVideos(Guid id, Guid clinicId)
    {
        return clientControllerService.GetVideos(this, id, clinicId);
    }

    [HttpGet("{id}/sounds/{clinicId}")]
    [Authorize(Roles = $"{SystemRoles.APP_MANAGER} , {SystemRoles.COMPANY_MANAGER}, {SystemRoles.COMPANY_OWNER}, {SystemRoles.CLINIC_OFFICER}, {SystemRoles.CLINIC_MANAGER}, {SystemRoles.CLIENT_MOBILE}")]
    public Task<IActionResult> GetSounds(Guid id, Guid clinicId)
    {
        return clientControllerService.GetSounds(this, id, clinicId);
    }
}
