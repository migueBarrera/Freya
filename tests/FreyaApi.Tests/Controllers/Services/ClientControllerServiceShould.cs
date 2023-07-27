using FreyaApi.Helpers;
using Models.Core.Clients;

namespace FreyaApi.Tests.Controllers.Services;

[TestFixture]
internal class ClientControllerServiceShould
{
    private ControllerBase controllerBase;
    private ClientControllerServiceBuilder builder;

    private string TEST_EMAIL = "test@gmail.com";
    private string TEST_PASS = "test";

    private string TEST_PASS_2 = "test2";

    [SetUp]
    public void SetUp()
    {
        builder = new ClientControllerServiceBuilder();
        controllerBase = new ControllerBaseBuilder().Build();
    }

    [Test]
    public async Task CanSignIn()
    {
        var service = builder
            .WithClient(TEST_EMAIL, Hash.Create(TEST_PASS))
            .Build();

        var request = new ClientSignInRequest()
        {
            Email = TEST_EMAIL,
            Pass = TEST_PASS,
        };

        var response = await service.SignIn(controllerBase, request);

        Assert.Multiple(() =>
        {
            Assert.That(response.Result, Is.InstanceOf<OkObjectResult>());
        });
    }

    [Test]
    public async Task SignInReturnNotFoundIfNotExist()
    {
        var service = builder
            .Build();

        var request = new ClientSignInRequest()
        {
            Email = TEST_EMAIL,
            Pass = TEST_PASS,
        };

        var response = await service.SignIn(controllerBase, request);

        Assert.Multiple(() =>
        {
            Assert.That(response.Result, Is.InstanceOf<NotFoundResult>());
        });
    }

    [Test]
    public async Task SignInReturnNotFoundIfPassDoesNotMatch()
    {
        var service = builder
            .WithClient(TEST_EMAIL, Hash.Create(TEST_PASS))
            .Build();

        var request = new ClientSignInRequest()
        {
            Email = TEST_EMAIL,
            Pass = TEST_PASS_2,
        };

        var response = await service.SignIn(controllerBase, request);

        Assert.Multiple(() =>
        {
            Assert.That(response.Result, Is.InstanceOf<NotFoundResult>());
        });
    }


    [Test]
    public async Task SignUpReturnConflictIfEmailExist()
    {
        var service = builder
            .WithClient(TEST_EMAIL, Hash.Create(TEST_PASS))
            .Build();

        var request = new ClientSignUpRequest()
        {
            Email = TEST_EMAIL,
            Pass = TEST_PASS,
        };

        var response = await service.SignUp(controllerBase, request);

        Assert.Multiple(() =>
        {
            Assert.That(response.Result, Is.InstanceOf<ConflictResult>());
        });
    }

    [Test]
    public async Task SignUp()
    {
        var service = builder
            .Build();

        var request = new ClientSignUpRequest()
        {
            Email = TEST_EMAIL,
            Pass = TEST_PASS,
        };

        var response = await service.SignUp(controllerBase, request);

        Assert.Multiple(() =>
        {
            Assert.That(response.Result, Is.InstanceOf<CreatedResult>());
        });
    }
}
