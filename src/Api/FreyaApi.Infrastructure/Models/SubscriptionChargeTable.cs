namespace FreyaApi.Infrastructure.Models;

[Table("SubscriptionCharges")]
public class SubscriptionChargeTable
{
    public Guid Id { get; set; }

    public Guid SubscriptionPaymentTableId { get; set; }

    public SubscriptionPaymentTable? SubscriptionPaymentTable { get; set; }

    public bool IsPaid { get; set; } = false;

    public string Error { get; set; } = string.Empty;

    public long Amount { get; set; } 

    public string StripeChargeId { get; set; } = string.Empty;

    public DateTime Created { get; set; } = DateTime.UtcNow;
}
