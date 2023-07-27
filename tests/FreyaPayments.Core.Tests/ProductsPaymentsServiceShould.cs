using System.Diagnostics.CodeAnalysis;

namespace FreyaPayments.Core.Tests;

public class ProductsPaymentsServiceShould
{
    private ProductsPaymentsServiceBuilder builder;

    private string TEST_PRODUCT_NAME = "Plan test product";
    private string TEST_PRODUCT_YEAR_NAME = "Plan test product with year price";
    private string TEST_PRODUCT_DESCRIPTION = "Esto es una descripción de prueba que puede borrarse";
    private string PRODUCT_EXIST_ID = "prod_N1x4WJiwhPkh5V";

    [SetUp]
    public void SetUp()
    {
        builder = new ProductsPaymentsServiceBuilder();
    }

    [Test]
    [Ignore("We can not delete a product by API, stripe limitation, so we ignore for no multime test products on Stripe")]
    [ExcludeFromCodeCoverage]
    public async Task CreteProduct()
    {
        var service = builder.Build();
        var productId = await service.CreateProductOnStripe(
            TEST_PRODUCT_NAME,
            TEST_PRODUCT_DESCRIPTION,
            "1000",
            2000,
            IntervalPay.month);

        Assert.That(productId, Is.Not.Null);
    }

    [Test]
    [Ignore("We can not delete a product by API, stripe limitation, so we ignore for no multime test products on Stripe")]
    [ExcludeFromCodeCoverage]
    public async Task CreteProductWithCentsPrice()
    {
        var priceCents = 2499;
        var service = builder.Build();
        var productId = await service.CreateProductOnStripe(
            TEST_PRODUCT_NAME + " Cents",
            TEST_PRODUCT_DESCRIPTION,
            "1000",
            priceCents,
            IntervalPay.month);

        var supscriptionFromStripe = await service.GetProductFromStripe(productId, null);
        Assert.Multiple(() =>
        {
            Assert.That(productId, Is.Not.Null);
            Assert.That(supscriptionFromStripe, Is.Not.Null);
            Assert.That(supscriptionFromStripe.AmountMonthly, Is.EqualTo(priceCents));
        });
    }

    [Test]
    [Ignore("We can not delete a product by API, stripe limitation, so we ignore for no multime test products on Stripe")]
    [ExcludeFromCodeCoverage]
    public async Task CreteProductAndAddAnnualPriceToo()
    {
        var service = builder.Build();
        var productId = await service.CreateProductOnStripe(
            TEST_PRODUCT_YEAR_NAME,
            TEST_PRODUCT_DESCRIPTION,
            "1000",
            2000,
            IntervalPay.month);

        var yearPriceId = await service.AddPrice(productId, 20000, IntervalPay.year);
        Assert.Multiple(() =>
        {
            Assert.That(productId, Is.Not.Null);
            Assert.That(yearPriceId, Is.Not.Null);
        });
    }

    [Test]
    [Ignore("We can not delete a product by API, stripe limitation, so we ignore for no multime test products on Stripe")]
    [ExcludeFromCodeCoverage]
    public async Task CreteProductAndAddAnnualPriceAndGenerateLinks()
    {
        var service = builder.Build();
        var productId = await service.CreateProductOnStripe(
            TEST_PRODUCT_YEAR_NAME,
            TEST_PRODUCT_DESCRIPTION,
            "1000",
            2000,
            IntervalPay.month);

        var yearPriceId = await service.AddPrice(productId, 20000, IntervalPay.year);

        var subsciptionPlanId = Guid.NewGuid();

        var uriYear = service.GenerateLinkByPrice(subsciptionPlanId, yearPriceId, false, "https://freyadevelop.es/", false);

        Assert.Multiple(() =>
        {
            Assert.That(productId, Is.Not.Null);
            Assert.That(yearPriceId, Is.Not.Null);
            Assert.That(Uri.IsWellFormedUriString(uriYear?.ToString(), UriKind.Absolute), Is.True);
        });
    }

    [Test]
    [Ignore("testing propouses")]
    [ExcludeFromCodeCoverage]
    public async Task ProductExist()
    {
        var service = builder.Build();
        var exist = await service.ProductExist(PRODUCT_EXIST_ID);

        Assert.That(exist, Is.True);
    }

    [Test]
    public async Task ProductNotExist()
    {
        var service = builder.Build();
        var exist = await service.ProductExist("INVENT_CONTENT");

        Assert.That(exist, Is.False);
    }

    [Test]
    public void CanGetProducts()
    {
        var service = builder.Build();
        var products = service.GetProductsFromStripe();

        Assert.That(products.Any(), Is.True);
    }
}
