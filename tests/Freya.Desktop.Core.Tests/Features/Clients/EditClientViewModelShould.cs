using Freya.Desktop.Core.Framework;
using Models.Core.Clients;

namespace Freya.Desktop.Core.Tests.Features.Clients;

[TestFixture]
public class EditClientViewModelShould
{
    private EditClientViewModelBuilder builder;

    [SetUp]
    public void SetUp()
    {
        builder = new EditClientViewModelBuilder();
    }

    [Test]
    public async Task Edit_Client_Expect_ClientDetailResponse_From_Session()
    {
        var viewModel = builder
            .Build();

        var clientDetail = new ClientDetailResponse()
        {
            Email = "Test@st.com",
            Id = Guid.NewGuid(),
        };

        builder.SessionService.Save(SESSION.KEY_CLIENT_DETAIL_SELECTED, clientDetail);

        await viewModel.OnAppearingAsync();
        Assert.Multiple(() =>
        {
            Assert.That(viewModel.EditClientModel.Id, Is.EqualTo(clientDetail.Id));
            Assert.That(viewModel.EditClientModel.ClientEmail, Is.EqualTo(clientDetail.Email));
        });
    }
}
