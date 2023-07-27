using Models.Core.Subscriptions;

namespace FreyaApi.Infrastructure.Mappers;

public static class SubscriptionsMapper
{
    public static PaymentsPlansForCompanyResponse ConverTo(SubscriptionPlanTable item)
    {
        return new PaymentsPlansForCompanyResponse()
        {
           CompanyId = item.CompanyId,
           Currency = item.SubscriptionProduct?.Currency ?? string.Empty,
           Id = item.Id,
           Description = item.SubscriptionProduct?.Description ?? string.Empty,
           IsActive = item.IsActive,
           Name = item.SubscriptionProduct?.Name ?? string.Empty,
           PaymentLinkAnnual = item.PaymentLinkAnnual,
           PaymentLinkMonthly = item.PaymentLinkMonthly,
           Size = item.SubscriptionProduct?.Size ?? 0L,
           ProductId = item.SubscriptionProduct?.ProductId ?? string.Empty,
           SubscriptionProductId = item.SubscriptionProductId,
           PriceIdMonthly = item.SubscriptionProduct?.PriceIdMonthly ?? string.Empty,
           AmountMonthly = item.SubscriptionProduct?.AmountMonthly ?? 0L,
           PriceIdAnnual = item.SubscriptionProduct?.PriceIdAnnual,
           AmountAnnual = item.SubscriptionProduct?.AmountAnnual,
        };
    }
    
    public static SubscriptionPaymentResponse ConverTo(SubscriptionPaymentTable item)
    {
        var charges = new List<SubscriptionChargeResponse>();
        if (item.SubscriptionCharges.Any())
        {
            charges = item.SubscriptionCharges.Select(x => SubscriptionsMapper.ConvertToResponse(x)).ToList();
        }

        return new SubscriptionPaymentResponse()
        {
           Currency = item.SubscriptionPlan?.SubscriptionProduct?.Currency ?? String.Empty,
           Id = item.Id,
           Description = item.SubscriptionPlan?.SubscriptionProduct?.Description ?? String.Empty,
           Name = item.SubscriptionPlan?.SubscriptionProduct?.Name ?? String.Empty,
           Size = item.SubscriptionPlan?.SubscriptionProduct?.Size ?? 0,
           ProductId = item.SubscriptionPlan?.SubscriptionProduct?.ProductId ?? String.Empty,
           ClinicId = item.ClinicId,
           StripeCustomerId = item.StripeCustomerId,
           StripeSubscriptionId = item.StripeSubscriptionId,
           SubscriptionPlanId = item.SubscriptionPlanId,
           State = item.State,
           Created = item.Created,
           Expire = item.Expire,
           PriceIdMonthly = item.SubscriptionPlan?.SubscriptionProduct?.PriceIdMonthly ?? String.Empty,
           AmountMonthly = item.SubscriptionPlan?.SubscriptionProduct?.AmountMonthly ?? 0,
           PriceIdAnnual = item.SubscriptionPlan?.SubscriptionProduct?.PriceIdAnnual ?? String.Empty,
           AmountAnnual = item.SubscriptionPlan?.SubscriptionProduct?.AmountAnnual ?? 0,
           IsAnnual = item.IsAnnual,
           IsMonthly = item.IsMonthly,
           SubscriptionCharges = charges,
        };
    }

    private static SubscriptionChargeResponse ConvertToResponse(SubscriptionChargeTable x)
    {
        return new SubscriptionChargeResponse()
        {
            Id = x.Id,
            Created = x.Created,
            Error = x.Error,
            IsPaid = x.IsPaid,
            SubscriptionPaymentTableId = x.SubscriptionPaymentTableId,
            Amount = x.Amount,
            StripChargeId = x.StripeChargeId,
        };
    }

    public static SubscriptionProductResponse ConvertToProductResponse(SubscriptionProductTable item)
    {
        return new SubscriptionProductResponse()
        {
            Id = item.Id,
            Currency = item.Currency,
            IsActive= item.IsActive,
            Description = item.Description,
            Name = item.Name,
            ProductId = item.ProductId,
            Size = item.Size,
            PriceIdMonthly = item.PriceIdMonthly,
            AmountMonthly = item.AmountMonthly,
            PriceIdAnnual = item.PriceIdAnnual,
            AmountAnnual = item.AmountAnnual,
        };
    }
}
