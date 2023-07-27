namespace Models.Core.Subscriptions;

public class SubscriptionPaymentResponse
{
    public Guid Id { get; set; }

    public Guid SubscriptionPlanId { get; set; }

    public Guid ClinicId { get; set; }

    public long Size { get; set; }

    public string StripeCustomerId { get; set; } = string.Empty;

    public string StripeSubscriptionId { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string ProductId { get; set; } = string.Empty;

    public string Currency { get; set; } = string.Empty;

    public long AmountMonthly { get; set; }

    public string PriceIdMonthly { get; set; } = string.Empty;

    public long? AmountAnnual { get; set; }

    public string? PriceIdAnnual { get; set; } = string.Empty;

    public bool IsMonthly { get; set; } = false;

    public bool IsAnnual { get; set; } = false;

    public SubscriptionStates State { get; set; }

    public DateTime Created { get; set; } = DateTime.MinValue;

    public DateTime Expire { get; set; } = DateTime.MinValue;

    public ICollection<SubscriptionChargeResponse> SubscriptionCharges { get; set; } = new List<SubscriptionChargeResponse>();
}
