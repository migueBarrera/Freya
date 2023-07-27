namespace Models.Core.Subscriptions;

public class SubscriptionProductResponse
{
    public Guid Id { get; set; }

    public long Size { get; set; }

    public bool IsActive { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string ProductId { get; set; } = string.Empty;

    public string Currency { get; set; } = string.Empty;

    public long AmountMonthly { get; set; }

    public string PriceIdMonthly { get; set; } = string.Empty;
    
    public long? AmountAnnual { get; set; }

    public string? PriceIdAnnual { get; set; } = string.Empty;
}
