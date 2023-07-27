namespace FreyaApi.Controllers;

[Route("api/[controller]/v1")]
public class PaymentsController : Controller
{
    private readonly PaymentsControllerService paymentsControllerService;

    public PaymentsController(
        PaymentsControllerService paymentsControllerService)
    {
        this.paymentsControllerService = paymentsControllerService;
    }

    [HttpGet]
    public ActionResult Index([FromQuery] string session_id)
    {
        return paymentsControllerService.Index(this, session_id);
    }
}
