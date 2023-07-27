namespace Models.Core.Subscriptions;

public class Subscription
{
    public string ProductId { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string Currency { get; set; } = string.Empty;

    public long Size{ get; set; }

    public long AmountMonthly { get; set; }

    public string PriceIdMonthly { get; set; } = string.Empty;
    
    public long? AmountAnnual { get; set; }

    public string? PriceIdAnnual { get; set; } = string.Empty;

}
