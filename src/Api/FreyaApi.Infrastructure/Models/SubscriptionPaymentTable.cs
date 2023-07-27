using Models.Core.Subscriptions;

namespace FreyaApi.Infrastructure.Models;

[Table("SubscriptionPayments")]
public class SubscriptionPaymentTable
{
    public Guid Id { get; set; }

    public Guid SubscriptionPlanId { get; set; }

    public SubscriptionPlanTable? SubscriptionPlan { get; set; }

    public Guid ClinicId { get; set; } 
    
    public ClinicTable? Clinic{ get; set; }  

    public string StripeSubscriptionId { get; set; } = string.Empty;

    public string StripeCustomerId { get; set; }  = string.Empty;

    public bool IsMonthly { get; set; }  = false;

    public bool IsAnnual { get; set; }  = false;

    public SubscriptionStates State { get; set; } = SubscriptionStates.NONE;

    public DateTime Created { get; set; } = DateTime.UtcNow;

    public DateTime Expire { get; set; } = DateTime.MinValue;

    public ICollection<SubscriptionChargeTable> SubscriptionCharges { get; set; } = new List<SubscriptionChargeTable>();
}
