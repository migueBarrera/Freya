namespace FreyaApi.Infrastructure.Models;

[Table("SubscriptionProducts")]
// Stripe reference 
public class SubscriptionProductTable
{
    public Guid Id { get; set; }

    public long Size { get; set; }

    public bool IsActive { get; set; }

    public string Name { get; set; } = string.Empty;    

    public string Description { get; set; } = string.Empty;    

    public string ProductId { get; set; } = string.Empty;

    public string Currency { get; set; } = string.Empty;

    //By default, stripe product does not contain AllowPromotionCodes property, but we need here because when generate links we need this parameter
    public bool AllowPromotionCodes { get; set; } = false;

    public long AmountMonthly { get; set; }

    public string PriceIdMonthly { get; set; } = string.Empty;
    
    public long? AmountAnnual { get; set; }

    public string? PriceIdAnnual { get; set; } = string.Empty;

    public ICollection<SubscriptionPlanTable> SubscriptionPlans { get; set; } = new List<SubscriptionPlanTable>();
}
