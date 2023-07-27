namespace FreyaApi.Controllers;

[Route("api/webhook")]
[ApiController]
public class StripeWebhookController : ControllerBase
{
    private ILogger<StripeWebhookController> logger;
    private readonly StripeWebhookControllerService stripeWebhookControllerService;

    public StripeWebhookController(
        ILogger<StripeWebhookController> logger, StripeWebhookControllerService stripeWebhookControllerService)
    {
        this.logger = logger;
        this.stripeWebhookControllerService = stripeWebhookControllerService;
    }

    [HttpPost]
    [Route("v1")]
    public async Task<IActionResult> Index()
    {
        return await stripeWebhookControllerService.ProcessStripeWebhooks(this);
    }
}
