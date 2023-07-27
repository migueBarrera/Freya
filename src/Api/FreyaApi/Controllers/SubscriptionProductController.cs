namespace FreyaApi.Controllers;

[Authorize]
[Route("api/[controller]/v1")]
[ApiController]
public class SubscriptionProductController : ControllerBase
{
    private readonly SubscriptionProductControllerServices subscriptionProductControllerServices;

    public SubscriptionProductController(SubscriptionProductControllerServices subscriptionProductControllerServices)
    {
        this.subscriptionProductControllerServices = subscriptionProductControllerServices;
    }

    [HttpGet]
    [Authorize(Roles = $"{SystemRoles.APP_MANAGER}")]
    public async Task<ActionResult<IEnumerable<SubscriptionProductResponse>>> GetPaymentProducts()
    {
        return await subscriptionProductControllerServices.GetPaymentProducts(this);
    }

    [HttpGet]
    [Route("generateFromStripe")]
    [Authorize(Roles = $"{SystemRoles.APP_MANAGER}")]
    public async Task<IActionResult> GenerateProductsFromStripe()
    {
        return await subscriptionProductControllerServices.GenerateProductsFromStripe(this);
    }

    [HttpPost]
    [Authorize(Roles = SystemRoles.APP_MANAGER)]
    public async Task<IActionResult> Create(SubscriptionProductCreateRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        return await subscriptionProductControllerServices.Create(this, request);
    }
}
