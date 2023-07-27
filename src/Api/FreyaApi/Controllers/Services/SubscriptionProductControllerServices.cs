namespace FreyaApi.Controllers.Services;

public class SubscriptionProductControllerServices
{
    private readonly IProductsPaymentsService productsPaymentsService;
    private readonly ISubscriptionProductRepository subscriptionProductRepository;

    public SubscriptionProductControllerServices(
        IProductsPaymentsService productsPaymentsService,
        ISubscriptionProductRepository subscriptionProductRepository)
    {
        this.productsPaymentsService = productsPaymentsService;
        this.subscriptionProductRepository = subscriptionProductRepository;
    }

    [HttpGet]
    [Authorize(Roles = $"{SystemRoles.APP_MANAGER}")]
    public async Task<ActionResult<IEnumerable<SubscriptionProductResponse>>> GetPaymentProducts(ControllerBase controller)
    {
        IEnumerable<SubscriptionProductTable> subscriptionsProducts = await subscriptionProductRepository.GetAllSubscriptionProducts();

        var response = subscriptionsProducts.Select(x => SubscriptionsMapper.ConvertToProductResponse(x));

        return controller.Ok(response);
    }

    [HttpGet]
    [Route("generateFromStripe")]
    [Authorize(Roles = $"{SystemRoles.APP_MANAGER}")]
    public async Task<IActionResult> GenerateProductsFromStripe(ControllerBase controller)
    {
        var subscriptions = productsPaymentsService.GetProductsFromStripe();

        List<SubscriptionProductTable> products = new List<SubscriptionProductTable>();
        foreach (var sub in subscriptions)
        {
            var subscriptionPlan = await GetProductByStripeAndMapToTable(sub.ProductId, allowPromotionCodes: true, null);
            if (subscriptionPlan != null)
            {
                products.Add(subscriptionPlan);
            }
        }

        if (products.Count == 0)
        {
            return controller.NotFound();
        }

        bool created = await subscriptionProductRepository.CreateSubscriptionProducts(products);
        if (!created)
        {
            return controller.NotFound();
        }

        return controller.Ok();
    }

    [HttpPost]
    [Authorize(Roles = SystemRoles.APP_MANAGER)]
    public async Task<IActionResult> Create(ControllerBase controller, SubscriptionProductCreateRequest request)
    {
        var productId = await productsPaymentsService.CreateProductOnStripe(request.Name, request.Description, request.SizeOnBytes, request.PriceMonthly, IntervalPay.month);
        if (string.IsNullOrEmpty(productId))
        {
            return controller.BadRequest();
        }

        string? priceAnnualId = null;
        if (request.PriceAnnual != null)
        {
            priceAnnualId = await productsPaymentsService.AddPrice(productId, request.PriceAnnual.Value, IntervalPay.year);
        }

        var subscriptionPlan = await GetProductByStripeAndMapToTable(productId, request.AllowPromotionCodes, priceAnnualId);
        if (subscriptionPlan == null)
        {
            return controller.BadRequest();
        }

        bool created = await subscriptionProductRepository.CreateSubscriptionProducts(subscriptionPlan);

        return controller.Created(string.Empty, subscriptionPlan);
    }

    private async Task<SubscriptionProductTable?> GetProductByStripeAndMapToTable(string productId, bool allowPromotionCodes, string? priceAnnualId)
    {
        var subs = await productsPaymentsService.GetProductFromStripe(productId, priceAnnualId);
        if (subs == null)
        {
            return null;
        }

        return new SubscriptionProductTable()
        {
            Id = Guid.NewGuid(),
            IsActive = true,
            ProductId = subs.ProductId,
            Size = subs.Size,
            Name = subs.Name,
            Description = subs.Description,
            Currency = subs.Currency,
            AllowPromotionCodes = allowPromotionCodes,
            PriceIdMonthly = subs.PriceIdMonthly,
            AmountMonthly = subs.AmountMonthly,
            PriceIdAnnual = subs.PriceIdAnnual,
            AmountAnnual = subs.AmountAnnual,
        };
    }
}
