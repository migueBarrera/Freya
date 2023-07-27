namespace FreyaApi.Tests.Services;

[TestFixture]
public class JwtSecurityTokenServiceShould
{
    private JwtSecurityTokenServiceBuilder builder;

    [SetUp]
    public void Setup()
    {
        builder = new JwtSecurityTokenServiceBuilder();
    }

    [Test]
    public void CanCreateATokenForAppManager()
    {
        var service = builder.Build();

        var token = service.BuildToken(new AppManagerTable());
        var refreshToken = service.BuildRefreshToken(new AppManagerTable());

        Assert.Multiple(() =>
        {
            Assert.That(token, Is.Not.Null);
            Assert.That(token, Is.Not.Empty);
            Assert.That(refreshToken, Is.Not.Null);
            Assert.That(refreshToken, Is.Not.Empty);
        });
    }

    [Test]
    public void CanCreateATokenForClient()
    {
        var service = builder.Build();

        var token = service.BuildToken(new ClientTable());
        var refreshToken = service.BuildRefreshToken(new ClientTable());

        Assert.Multiple(() =>
        {
            Assert.That(token, Is.Not.Null);
            Assert.That(token, Is.Not.Empty);
            Assert.That(refreshToken, Is.Not.Null);
            Assert.That(refreshToken, Is.Not.Empty);
        });
    }

    [Test]
    public void CanCreateATokenForEmployee()
    {
        var service = builder.Build();

        var token = service.BuildToken(new EmployeeTable());
        var refreshToken = service.BuildRefreshToken(new EmployeeTable());

        Assert.Multiple(() =>
        {
            Assert.That(token, Is.Not.Null);
            Assert.That(token, Is.Not.Empty);
            Assert.That(refreshToken, Is.Not.Null);
            Assert.That(refreshToken, Is.Not.Empty);
        });
    }

    [Test]
    public void CanValidateRefreshToken()
    {
        var service = builder.Build();
        var id = Guid.NewGuid();

        var refreshToken = service.BuildRefreshToken(new EmployeeTable() { Id = id });

        var isValid = service.ValidateToken(refreshToken, out var claims);

        var claimId = claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value!;
        Assert.Multiple(() =>
        {
            Assert.That(isValid, Is.True);
            Assert.That(id.ToString(), Is.EqualTo(claimId));
        });
    }
}
