using Models.Core.Clients;

namespace Freya.Desktop.Core.Tests.Features.Clients;

[TestFixture]
public class ClientItemViewModelShould
{
    private ClientItemViewModelBuilder builder;

    [SetUp] public void SetUp()
    {
        builder = new ClientItemViewModelBuilder();
    }

    [Test]
    public async Task ClientItemViewModel_Expected_ClientResponse_OnAprearingAsync()
    {
        var viewModel = builder.Build();

        var clientResponse = new ClientResponse()
        {
            Email = "Test@gmail.com",
            
        };

        await viewModel.OnAppearingAsync(clientResponse);

        Assert.That(viewModel.Email, Is.EqualTo(clientResponse.Email));
    }
}
