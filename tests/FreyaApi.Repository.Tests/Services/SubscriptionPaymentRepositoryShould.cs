using Models.Core.Subscriptions;

namespace FreyaApi.Repository.Tests.Services;

[TestFixture]
internal class SubscriptionPaymentRepositoryShould
{
    private SubscriptionPaymentRepositoryBuilder builder;

    [SetUp]
    public void SetUp()
    {
        builder = new SubscriptionPaymentRepositoryBuilder();
    }

    [Test]
    public async Task CanCreateSubscriptionPayment()
    {
        var service = builder.Build();

        var payment = new SubscriptionPaymentTable()
        {
            Id = Guid.NewGuid(),
            State = SubscriptionStates.ACTIVE,
        };

        var created  = await service.Create(payment);

        Assert.That(created, Is.True);
    }
    
    [Test]
    public async Task CanUpdateSubscriptionPayment()
    {
        var service = builder.Build();

        var payment = new SubscriptionPaymentTable()
        {
            Id = Guid.NewGuid(),
            State = Models.Core.Subscriptions.SubscriptionStates.NONE,
        };

        builder.dbContext!.SubscriptionPayments?.Add(payment);
        builder.dbContext!.SaveChanges();

        var updated = await service.UpdateState(payment.Id, Models.Core.Subscriptions.SubscriptionStates.ACTIVE);

        Assert.That(updated, Is.True);
    }
    
    [Test]
    public async Task CanGetSubscriptionPayment()
    {
        var service = builder.Build();

        var product = new SubscriptionProductTable()
        {
            Id = Guid.NewGuid(),
        };
        var plan = new SubscriptionPlanTable()
        {
            Id = Guid.NewGuid(),
            SubscriptionProductId = product.Id,
        };
        var payment = new SubscriptionPaymentTable()
        {
            Id = Guid.NewGuid(),
            State = SubscriptionStates.NONE,
            ClinicId = Guid.NewGuid(),
            SubscriptionPlanId = plan.Id,
        };

        builder.dbContext!.SubscriptionProducts?.Add(product);
        builder.dbContext.SubscriptionPlans?.Add(plan);
        builder.dbContext.SubscriptionPayments?.Add(payment);
        builder.dbContext.SaveChanges();

        var databasePayment = await service.GetSubscriptionPayment(payment.Id);

        Assert.That(databasePayment, Is.Not.Null);
    }
    
    [Test]
    public async Task CanRetryUpdatedSubscriptionPayment()
    {
        var service = builder.Build();

        var product = new SubscriptionProductTable()
        {
            Id = Guid.NewGuid(),
        };
        var plan = new SubscriptionPlanTable()
        {
            Id = Guid.NewGuid(),
            SubscriptionProductId = product.Id,
        };
        var payment = new SubscriptionPaymentTable()
        {
            Id = Guid.NewGuid(),
            State = SubscriptionStates.NONE,
            ClinicId = Guid.NewGuid(),
            SubscriptionPlanId = plan.Id,
        };

        builder.dbContext!.SubscriptionProducts?.Add(product);
        builder.dbContext.SubscriptionPlans?.Add(plan);
        builder.dbContext.SubscriptionPayments?.Add(payment);
        builder.dbContext.SaveChanges();

        var updated = await service.UpdateState(payment.Id, SubscriptionStates.ACTIVE);

        var paymentDatabse = await service.GetCurrentActiveClinicSubscription(payment.ClinicId);
        Assert.Multiple(() =>
        {
            Assert.That(updated, Is.True);
            Assert.That(paymentDatabse?.State, Is.EqualTo(SubscriptionStates.ACTIVE));
        });
    }
}
