namespace FreyaApi.Controllers.Services;

public class PaymentsControllerService
{
    public PaymentsControllerService()
    {
    }

    public ActionResult Index(Controller controller, string session_id)
    {
        return controller.View("~/Views/Index.cshtml");
    }
}
