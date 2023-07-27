namespace FreyaApi.Controllers;

[Route("api/[controller]/v1")]
[ApiController]
public class SubscriptionPaymentController : ControllerBase
{
    private readonly SubscriptionPaymentControllerService subscriptionPaymentControllerService;

    public SubscriptionPaymentController(
        SubscriptionPaymentControllerService subscriptionPaymentControllerService)
    {
        this.subscriptionPaymentControllerService = subscriptionPaymentControllerService;
    }

    [HttpGet]
    [Authorize(Roles = $"{SystemRoles.APP_MANAGER} , {SystemRoles.COMPANY_MANAGER}, {SystemRoles.COMPANY_OWNER}, {SystemRoles.CLINIC_OFFICER}, {SystemRoles.CLINIC_MANAGER}")]
    [Route("byclinic/{clinicId}")]
    public async Task<ActionResult<IEnumerable<SubscriptionPaymentResponse>>> GetSubscriptionPayments(Guid clinicId)
    {
        return await subscriptionPaymentControllerService.GetSubscriptionPayments(this, clinicId);
    }
    
    [HttpGet]
    [Authorize(Roles = $"{SystemRoles.APP_MANAGER} , {SystemRoles.COMPANY_MANAGER}, {SystemRoles.COMPANY_OWNER}, {SystemRoles.CLINIC_OFFICER}, {SystemRoles.CLINIC_MANAGER}")]
    [Route("{id}")]
    public async Task<ActionResult<SubscriptionPaymentResponse>> GetSubscriptionPaymentById(Guid id)
    {
        return await subscriptionPaymentControllerService.GetSubscriptionPayment(this, id);
    }

    [HttpDelete]
    [Authorize(Roles = $"{SystemRoles.APP_MANAGER} , {SystemRoles.COMPANY_MANAGER}, {SystemRoles.COMPANY_OWNER}, {SystemRoles.CLINIC_OFFICER}, {SystemRoles.CLINIC_MANAGER}")]
    [Route("{subscriptionId}")]
    public async Task<ActionResult<bool>> CancelSubscription(Guid subscriptionId)
    {
        return await subscriptionPaymentControllerService.CancelSubscription(this, subscriptionId);
    }
}
