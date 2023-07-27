namespace Models.Core.Subscriptions;

public class SubscriptionPlan
{
    public Guid Id { get; set; }

    public Uri PaymentLink { get; set; } = new Uri("about:blank");

    public bool IsActive { get; set; }

    public Guid CompanyId { get; set; }

    public Guid SubscriptionProductId { get; set; }
}
