using Models.Core.Subscriptions;

namespace FreyaApi.Tests.Controllers.Services.Builders;

internal class SubscriptionProductControllerServiceBuilder
{
    private readonly Mock<ISubscriptionProductRepository> subscriptionProductRepository;
    private readonly Mock<IProductsPaymentsService> productsPaymentsService;

    private string productId = "PRODUCT_ID";
    private Subscription? subcriptionFromStripe = new Subscription();

    public SubscriptionProductControllerServiceBuilder()
    {
        subscriptionProductRepository = new Mock<ISubscriptionProductRepository>();
        productsPaymentsService = new Mock<IProductsPaymentsService>();
    }

    public SubscriptionProductControllerServices Build()
    {
        productsPaymentsService.Setup(x => x.CreateProductOnStripe(
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<long>(),
            It.IsAny<string>()))
            .ReturnsAsync(productId);

        productsPaymentsService
            .Setup(x => x.GetProductFromStripe(productId, null))
            .ReturnsAsync(subcriptionFromStripe!);

        return new SubscriptionProductControllerServices(productsPaymentsService.Object, subscriptionProductRepository.Object);
    }

    internal SubscriptionProductControllerServiceBuilder WithCanNotCreateProductOnStripe()
    {
        productId = string.Empty;

        return this;
    }

    internal SubscriptionProductControllerServiceBuilder WithCanNotReadProductOnStripe()
    {
        subcriptionFromStripe = null;

        return this;
    }
}
