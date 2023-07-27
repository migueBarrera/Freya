
namespace FreyaApi.Repository.Interfaces;

public interface ISubscriptionPlanRepository
{
    Task CreateSubscriptionPlans(List<SubscriptionPlanTable> listSubscriptionPlan);

    Task<IEnumerable<SubscriptionPlanTable>> GetPlansForCompany(Guid companyId);
}
