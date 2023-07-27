namespace Models.Core.Subscriptions;

public class PaymentsPlansForCompanyResponse
{
    public Guid Id { get; set; }

    public Uri PaymentLinkMonthly { get; set; } = new Uri("about:blank");

    public Uri? PaymentLinkAnnual { get; set; }

    public bool IsActive { get; set; }

    public Guid CompanyId { get; set; }

    public Guid SubscriptionProductId { get; set; }

    public long Size { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string ProductId { get; set; } = string.Empty;

    public string Currency { get; set; } = string.Empty;

    public long AmountMonthly { get; set; }

    public string PriceIdMonthly { get; set; } = string.Empty;

    public long? AmountAnnual { get; set; }

    public string? PriceIdAnnual { get; set; } = string.Empty;
}
