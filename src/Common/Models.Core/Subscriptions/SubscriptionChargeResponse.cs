namespace Models.Core.Subscriptions;

public class SubscriptionChargeResponse
{
    public Guid Id { get; set; }

    public Guid SubscriptionPaymentTableId { get; set; }

    public bool IsPaid { get; set; } = false;

    public string Error { get; set; } = string.Empty;

    public long Amount { get; set; }

    public string StripChargeId { get; set; } = string.Empty;

    public DateTime Created { get; set; } = DateTime.UtcNow;
}
