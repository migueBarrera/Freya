namespace FreyaApi.Tests.Controllers.Services;

[TestFixture]
public class TokenControllerServiceShould
{
    private JwtSecurityTokenService jwtService;
    private TokenControllerService controller;
    private ControllerBase controllerBase;

    [SetUp]
    public void Setup()
    {
        controllerBase = new ControllerBaseBuilder().Build();
        jwtService = new JwtSecurityTokenServiceBuilder().Build();
        controller = new TokenControllerService(jwtService);
    }

    [Test]
    public void ReturnOkIfToken()
    {
        var refreshToken = jwtService.BuildRefreshToken(new List<Claim>()
        {
            new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
        });

        var request = new RefreshTokenRequest
        {
            RefreshToken = refreshToken,
        };

        var response = controller.RefresTokenClient(controllerBase, request);

        var resultObject = response.GetObjectResult();

        Assert.Multiple(() =>
        {
            Assert.That(response.Result, Is.InstanceOf<OkObjectResult>());
            Assert.That(resultObject.RefreshToken, Is.Not.Empty);
            Assert.That(resultObject.Token, Is.Not.Empty);
        });
    }

    [Test]
    public void ReturnUnauthorizedIfTokenIsEmptyClaims()
    {
        var refreshToken = jwtService.BuildRefreshToken(Enumerable.Empty<Claim>());
        var request = new RefreshTokenRequest
        {
            RefreshToken = refreshToken,
        };
        var response = controller.RefresTokenClient(controllerBase, request);

        Assert.Multiple(() =>
        {
            Assert.That(response.Result, Is.InstanceOf<UnauthorizedResult>());
        });
    }
}
