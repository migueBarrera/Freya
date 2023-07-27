using Microsoft.Extensions.Logging;
using Stripe;

namespace FreyaPayments.Core;

public class ProductsPaymentsService : IProductsPaymentsService
{
    private ILogger<ProductsPaymentsService> logger;

    public ProductsPaymentsService(ILogger<ProductsPaymentsService> logger)
    {
        this.logger = logger;
    }

    public Uri GenerateLinkByPrice(Guid subscriptionPlanId, string priceId, bool allowPromotionCodes, string baseUri, bool isMonthly)
    {
        var metadata = new Dictionary<string, string>
        {
            { "subscriptionPlanId", subscriptionPlanId.ToString() },
            { "isMonthly", isMonthly.ToString() },
            { "isAnnual", (!isMonthly).ToString() },
        };

        var uri = $"{baseUri}api/Payments/v1";
        var options = new PaymentLinkCreateOptions
        {
            LineItems = new List<PaymentLinkLineItemOptions>
            {
                new PaymentLinkLineItemOptions
                {
                    Price = priceId,
                    Quantity = 1,
                },
            },
            AfterCompletion = new PaymentLinkAfterCompletionOptions
            {
                Type = "redirect",
                Redirect = new PaymentLinkAfterCompletionRedirectOptions
                {
                    Url = uri + "/?session_id={CHECKOUT_SESSION_ID}",
                },
            },
            Metadata = metadata,
            AllowPromotionCodes = allowPromotionCodes,
            AutomaticTax = new PaymentLinkAutomaticTaxOptions()
            {
                Enabled = true,
            },
        };
        var paymentLinkService = new PaymentLinkService();
        var paymentLink = paymentLinkService.Create(options);

        return new Uri(paymentLink.Url);
    }

    public async Task<Models.Core.Subscriptions.Subscription> GetProductFromStripe(string productId, string? priceAnnualId)
    {
        var priceService = new PriceService();
        var service = new ProductService();

        var product = await service.GetAsync(productId);
        product.Metadata.TryGetValue("size", out var metadadta);
        long.TryParse(metadadta, out var size);

        var price = priceService.Get(product.DefaultPriceId);
        bool amountIsNull = price?.UnitAmount == null;
        long amount = amountIsNull
            ? 0L
            : (long) (price!.UnitAmount!);

        long? amountAnnual = null;
        if (!string.IsNullOrEmpty(priceAnnualId))
        {
            var priceAnnual = priceService.Get(priceAnnualId);
            bool amountIsNullAnnual = priceAnnual?.UnitAmount == null;
            amountAnnual = amountIsNullAnnual
                ? 0L
                : (long)(priceAnnual!.UnitAmount!);
        }

        return new Models.Core.Subscriptions.Subscription()
        {
            ProductId = product.Id,
            Name = product.Name,
            Description = product.Description,
            Currency = price?.Currency ?? string.Empty,
            Size = size,
            PriceIdMonthly = price?.Id ?? string.Empty,
            AmountMonthly = amount,
            AmountAnnual = amountAnnual,
            PriceIdAnnual = priceAnnualId,
        };
    }

    public IEnumerable<Models.Core.Subscriptions.Subscription> GetProductsFromStripe()
    {
        var subscriptions = new List<Models.Core.Subscriptions.Subscription>();
        var options = new ProductListOptions
        {
            Limit = 10,
        };

        var service = new ProductService();
        StripeList<Product> products = service.List(
          options);

        var priceService = new PriceService();

        foreach (var item in products)
        {
            item.Metadata.TryGetValue("size", out var metadadta);
            long.TryParse(metadadta, out var size);
            var price = priceService.Get(item.DefaultPriceId);
            bool amountIsNull = price?.UnitAmount == null;
            long amount = amountIsNull
                ? 0L
                : (long)(price!.UnitAmount! / 100);

            subscriptions.Add(new Models.Core.Subscriptions.Subscription()
            {
                ProductId = item.Id,
                Name = item.Name,
                Description = item.Description,
                Currency = price?.Currency ?? string.Empty,
                PriceIdMonthly = price?.Id ?? string.Empty,
                Size = size,
                AmountMonthly = amount,
            });
        }

        return subscriptions;
    }

    public async Task<string> CreateProductOnStripe(
        string productName, 
        string description, 
        string sizeOnBytes, 
        long amountCent, 
        string interval)
    {
        var options = new ProductCreateOptions
        {
            Name = productName,
            Description = description,
            DefaultPriceData = new ProductDefaultPriceDataOptions()
            {
                UnitAmount = amountCent,
                Currency = "EUR",
                TaxBehavior = "inclusive",
                Recurring = new ProductDefaultPriceDataRecurringOptions()
                {
                    Interval = interval,
                },
            },
            
            Metadata = new Dictionary<string, string>()
            {
                { "size", sizeOnBytes},
            },
        };

        try
        {
            var service = new ProductService();
            var product = await service.CreateAsync(options);
            return product.Id;
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return string.Empty;
        }
    }

    public async Task<bool> ProductExist(string productId)
    {
        try
        {
            var service = new ProductService();
            var product = await service.GetAsync(productId);
            return product != null;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<string> AddPrice(string productId, long amountCent, string interval)
    {
        try
        {
            var options = new PriceCreateOptions
            {
                UnitAmount = amountCent,
                Currency = "EUR",
                TaxBehavior = "inclusive",
                Recurring = new PriceRecurringOptions
                {
                    Interval = interval,
                },
                Product = productId,
            };
            var service = new PriceService();
            var newPrice = await service.CreateAsync(options);
            return newPrice.Id;
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return string.Empty;
        }
    }
}
