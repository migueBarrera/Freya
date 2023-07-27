namespace FreyaApi.Controllers.Services;

public class SubscriptionPlanControllerServices
{
    private readonly ISubscriptionPlanRepository subscriptionPlanRepository;

    public SubscriptionPlanControllerServices(
        ISubscriptionPlanRepository subscriptionPlanRepository)
    {
        this.subscriptionPlanRepository = subscriptionPlanRepository;
    }

    public async Task<ActionResult<IEnumerable<PaymentsPlansForCompanyResponse>>> GetPaymentPlansForCompany(ControllerBase controller, Guid companyId)
    {
        IEnumerable<SubscriptionPlanTable> paymentsPlans = await subscriptionPlanRepository.GetPlansForCompany(companyId);

        var response = paymentsPlans.Select(x => SubscriptionsMapper.ConverTo(x));

        return controller.Ok(response);
    }
}
