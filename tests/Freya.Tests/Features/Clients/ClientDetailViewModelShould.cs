using Freya.Desktop.Core.Features.Clients.Models;
using Freya.Desktop.Core.Framework;
using Models.Core.Clients;

namespace Freya.Tests.Features.Clients;

[TestFixture]
public class ClientDetailViewModelShould
{
    private ClientDetailViewModelBuilder builder;

    [SetUp]
    public void SetUp()
    {
        builder = new ClientDetailViewModelBuilder();
    }

    [Test]
    public async Task Client_Detail_Expect_ClientResponse_From_Session()
    {
        var viewModel = builder
            .SaveOnSession<ClientResponse>(SESSION.KEY_CLIENT_SELECTED, new ClientResponse())
            .WithGetClientDetails()
            .Build();

        await viewModel.OnAppearingAsync();
    }
    
    [Test]
    public async Task Client_Detail_Expect_ClinicId_From_Session()
    {
        var viewModel = builder
            .SaveOnSession<Guid>(SESSION.KEY_CLINIC_ID_SELECTED, Guid.NewGuid())
            .SaveOnSession<ClientResponse>(SESSION.KEY_CLIENT_SELECTED, new ClientResponse())
            .WithGetClientDetails()
            .Build();

        await viewModel.OnAppearingAsync();
    }

    [Test]
    public void Go_To_Edit_Client_Require_Add_ClientDetailResponse_To_Session()
    {
        var viewModel = builder.Build();

        viewModel.Client = new ClientDetailModel();
        viewModel.EditClientCommand.Execute(null);

        builder.sessionService.Verify(x => 
            x.Save(It.Is<string>(x => x == SESSION.KEY_CLIENT_DETAIL_SELECTED), It.IsAny<ClientDetailResponse>()), 
            Times.Once);
    }
}
