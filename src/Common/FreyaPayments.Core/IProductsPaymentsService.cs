namespace FreyaPayments.Core;

public interface IProductsPaymentsService
{
    IEnumerable<Models.Core.Subscriptions.Subscription> GetProductsFromStripe();

    Task<Models.Core.Subscriptions.Subscription> GetProductFromStripe(string productId, string? priceAnnualId);

    Uri GenerateLinkByPrice(Guid subscriptionPlanId, string priceId, bool allowPromotionCodes, string baseUri, bool isMonthly);

    Task<string> CreateProductOnStripe(string productName, string description, string sizeOnBytes, long amountCent, string interval);

    Task<string> AddPrice(string productId, long amountCent, string interval);

}

public static class IntervalPay
{
    public static string year = "year";
    public static string month = "month";
}
