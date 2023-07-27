namespace Models.Core.Subscriptions;

public class SubscriptionProduct
{
    public Guid Id { get; set; }

    public long Size { get; set; }

    public bool IsActive { get; set; }

    public string ProductId { get; set; } = string.Empty;
}
