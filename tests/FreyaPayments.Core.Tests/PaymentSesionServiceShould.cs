using System.Diagnostics.CodeAnalysis;

namespace FreyaPayments.Core.Tests;

public class PaymentSesionServiceShould
{
    private PaymentSesionServiceBuilder builder;

    [SetUp]
    public void SetUp()
    {
        builder = new PaymentSesionServiceBuilder();
    }

    [Test]
    [Ignore("For testing propouses")]
    [ExcludeFromCodeCoverage]
    public async Task CancelSubscription()
    {
        var service = builder.Build();

        var response = await service.CancelSubscriptionNow("sub_1MEskpHvyF8YYY2NRCmA8VgK");

        Assert.IsTrue(response);
    }
    
    //No podemos obtener un session id facilmente
    //[Test]
    //[TestCase("sessionId", "subscriptionPlanExpected")]
    ////[Ignore("For testing propouses")]
    //public async Task GetSubscriptionPlanIdFromSessionId(string sessionId, string subscriptionPlanExpectedString)
    //{
    //    var service = builder.Build();

    //    var response = await service.GetSubscriptionPlanIdFromSessionId(sessionId);

    //    var subscriptionPlanId = Guid.Parse(subscriptionPlanExpectedString);
    //    Assert.That(response.SubscriptionPlainId, Is.EqualTo(subscriptionPlanId));
    //}
}
