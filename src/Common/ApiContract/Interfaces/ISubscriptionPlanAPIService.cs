using Models.Core.Subscriptions;

namespace ApiContract.Interfaces;

public interface ISubscriptionPlanAPIService
{
    [Get("/api/SubscriptionPlan/v1/{companyId}")]
    Task<IEnumerable<PaymentsPlansForCompanyResponse>> GetSubscriptionPlan(Guid companyId);
}
