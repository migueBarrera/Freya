using Freya.Desktop.Core.Framework;
using Models.Core.Subscriptions;

namespace Freya.Tests.Features.Subscriptions;

[TestFixture]
public class CheckoutViewModelShould
{
    private CheckoutViewModelBuilder builder;

    [SetUp]
    public void SetUp()
    {
        builder = new CheckoutViewModelBuilder();
    }

    [Test]
    public async Task Checkout_Expect_PaymentsPlansForCompanyResponse_From_Session()
    {
        var viewmodel = builder
            .WithEmployee()
            .WithCurrentClinic()
            .Build();

        builder.sessionService.Save(SESSION.KEY_SUBSCRIPTION_SELECTED, new PaymentsPlansForCompanyResponse());

        await viewmodel.OnAppearingAsync();

        Assert.That(viewmodel.Source.ToString(), Is.Not.EqualTo("about:blank"));
    }
    
    [Test]
    public async Task Checkout_Go_Back_If_Not_Has_A_Current_Employee()
    {
        var viewmodel = builder
            .WithCurrentClinic()
            .Build();

        builder.sessionService.Save(SESSION.KEY_SUBSCRIPTION_SELECTED, new PaymentsPlansForCompanyResponse());

        await viewmodel.OnAppearingAsync();

        builder.navigationService.Verify(x => x.BackAsync(), Times.Once());
    }
    
    [Test]
    public async Task Checkout_Go_Back_If_Not_Has_A_Clinic()
    {
        var viewmodel = builder
            .WithEmployee()
            .Build();

        builder.sessionService.Save(SESSION.KEY_SUBSCRIPTION_SELECTED, new PaymentsPlansForCompanyResponse());

        await viewmodel.OnAppearingAsync();

        builder.navigationService.Verify(x => x.BackAsync(), Times.Once());
    }
}
