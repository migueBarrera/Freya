using Models.Core.Subscriptions;

namespace FreyaApi.Tests.Controllers.Services;

[TestFixture]
public class SubscriptionProductControllerServiceShould
{
    private SubscriptionProductControllerServiceBuilder builder;
    private ControllerBase controllerBase;

    [SetUp]
    public void Setup()
    {
        controllerBase = new ControllerBaseBuilder().Build();
        builder = new SubscriptionProductControllerServiceBuilder();
    }

    [Test]
    public async Task CanCreateANewSubscription()
    {
        var controller = builder.Build();

        var request = new SubscriptionProductCreateRequest();

        var response = await controller.Create(controllerBase, request);

        var result = response as CreatedResult;

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
        });
    }

    [Test]
    public async Task ReturnBadRequestIfCreateProductReturnEmpty()
    {
        var controller = builder
            .WithCanNotCreateProductOnStripe()
            .Build();

        var request = new SubscriptionProductCreateRequest();

        var response = await controller.Create(controllerBase, request);

        var result = response as BadRequestResult;

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
        });
    }

    [Test]
    public async Task ReturnBadRequestIfCanNotReturnProductFromStripe()
    {
        var controller = builder
            .WithCanNotReadProductOnStripe()
            .Build();

        var request = new SubscriptionProductCreateRequest();

        var response = await controller.Create(controllerBase, request);

        var result = response as BadRequestResult;

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
        });
    }
}
