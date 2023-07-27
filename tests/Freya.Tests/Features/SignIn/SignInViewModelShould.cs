using Freya.Features.Clients;

namespace Freya.Tests.Features.SignIn;

[TestFixture]
public class SignInViewModelShould
{
    private string TEST_MAIL = "test@test.com";
    private string TEST_PASS = "test";
    private SignInViewModelBuilder builder;

    [SetUp]
    public void SetUp() 
    {
        builder = new SignInViewModelBuilder();
    }

    [Test]
    public async Task Call_Sign_Service_If_Has_Valid_Values()
    {
        var viewModel = builder
            .Build();

        viewModel.Email = TEST_MAIL;
        viewModel.Pass = TEST_PASS;

        await viewModel.EnterCommand.ExecuteAsync();

        builder.signInService.Verify(
            x => x.SignInAsync(
                It.Is<string>(y => y.Equals(TEST_MAIL)),
                It.Is<string>(y => y.Equals(TEST_PASS))), 
            Times.Once);
    }
    
    [Test]
    public async Task Not_Call_Sign_Service_If_Not_Has_Valid_Values()
    {
        var viewModel = builder.Build();

        viewModel.Email = string.Empty;
        viewModel.Pass = string.Empty;

        await viewModel.EnterCommand.ExecuteAsync();

        builder.signInService.Verify(
            x => x.SignInAsync(
                It.Is<string>(y => y.Equals(TEST_MAIL)),
                It.Is<string>(y => y.Equals(TEST_PASS))), 
            Times.Never);
    }
    
    [Test]
    public async Task Go_To_Clients_Page_If_Success_SignIn()
    {
        var viewModel = builder
            .SignInSuccess(TEST_MAIL, TEST_PASS)
            .Build();

        viewModel.Email = TEST_MAIL;
        viewModel.Pass = TEST_PASS;

        await viewModel.EnterCommand.ExecuteAsync();

        builder.navigationService.Verify(
            x => x.NavigateTo(It.Is<Type>(y => y == typeof(ClientsPage)), true), 
            Times.Once);
    }
}
