namespace FreyaApi.Controllers;

[Authorize]
[Route("api/[controller]/v1")]
[ApiController]
public class SubscriptionPlanController : ControllerBase
{
    private readonly SubscriptionPlanControllerServices subscriptionPlanControllerServices;

    public SubscriptionPlanController(SubscriptionPlanControllerServices subscriptionPlanControllerServices)
    {
        this.subscriptionPlanControllerServices = subscriptionPlanControllerServices;
    }

    [HttpGet]
    [Authorize(Roles = $"{SystemRoles.APP_MANAGER} , {SystemRoles.COMPANY_MANAGER}, {SystemRoles.COMPANY_OWNER}, {SystemRoles.CLINIC_OFFICER}, {SystemRoles.CLINIC_MANAGER}")]
    [Route("{companyId}")]
    public async Task<ActionResult<IEnumerable<PaymentsPlansForCompanyResponse>>> GetPaymentPlansForCompany(Guid companyId)
    {
        return await subscriptionPlanControllerServices.GetPaymentPlansForCompany(this, companyId);
    }
}
