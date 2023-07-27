namespace FreyaApi.Infrastructure.Models;

[Table("SubscriptionPlans")]
// Stripe Relation With A Company
public class SubscriptionPlanTable
{
    public  Guid Id { get; set; }

    public Uri PaymentLinkMonthly { get; set; } = new Uri("about:blank");

    public Uri? PaymentLinkAnnual { get; set; }

    public bool IsActive { get; set; }

    public Guid CompanyId { get; set; }

    public CompanyTable? Company { get; set; }

    public Guid SubscriptionProductId { get; set; }

    public SubscriptionProductTable? SubscriptionProduct { get; set; }

    public ICollection<SubscriptionPaymentTable> SubscriptionPayments { get; set; } = new List<SubscriptionPaymentTable>();
}
