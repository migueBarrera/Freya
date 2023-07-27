using Microsoft.AspNetCore.Http;
using System.Text;

namespace FreyaApi.Tests.Controllers.Services;

[TestFixture]
public class StripeWebhookControllerServiceShould
{
    private StripeWebhookControllerServiceBuilder builder;
    private ControllerBase controllerBase;

    [SetUp]
    public void SetUp()
    {
        builder = new StripeWebhookControllerServiceBuilder();
        controllerBase = new ControllerBaseBuilder().Build();
    }

    [Test]
    public async Task BadResponseIfNullBody()
    {
        var service = builder
            .Build();

        var request = new Mock<HttpRequest>();
        request.Setup(x => x.Body).Returns(Stream.Null);

        var httpContext = Mock.Of<HttpContext>(_ =>
            _.Request == request.Object
        );

        controllerBase = new ControllerBaseBuilder().Build(httpContext);

        var response = await service.ProcessStripeWebhooks(controllerBase);

        var result = response as BadRequestResult;

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
        });
    }
    
    [Test]
    public async Task BadResponseIfInvalidBody()
    {
        var service = builder
            .Build();

        MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes("INVALID_VALUE"));

        var request = new Mock<HttpRequest>();
        request.Setup(x => x.Body).Returns(ms);

        var httpContext = Mock.Of<HttpContext>(_ =>
            _.Request == request.Object
        );

        controllerBase = new ControllerBaseBuilder().Build(httpContext);

        var response = await service.ProcessStripeWebhooks(controllerBase);

        var result = response as BadRequestResult;

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
        });
    }
}
