namespace Models.Core.Subscriptions;

public class SubscriptionProductCreateRequest
{
    public bool IsActive { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string SizeOnBytes { get; set; } = string.Empty;

    public bool AllowPromotionCodes { get; set; }

    public long PriceMonthly { get; set; }

    public long? PriceAnnual { get; set; }
}
