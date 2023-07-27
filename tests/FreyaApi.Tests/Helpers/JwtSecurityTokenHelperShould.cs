using FreyaApi.Helpers;

namespace FreyaApi.Tests.Helpers;

[TestFixture]
public class JwtSecurityTokenHelperShould
{
    [Test]
    public void CanCreateASimpleToken()
    {
        var token = JwtSecurityTokenHelper.BuildToken(
            Consts.Test_Key,
            Enumerable.Empty<Claim>(),
            1);

        Assert.Multiple(() =>
        {
            Assert.That(token, Is.Not.Null);
            Assert.That(token, Is.Not.Empty);
        });
    }

    [Test]
    public void CanValidateASimpleToken()
    {
        var token = JwtSecurityTokenHelper.BuildToken(
            Consts.Test_Key,
            Enumerable.Empty<Claim>(),
            1);

        var isValid = JwtSecurityTokenHelper.ValidateToken(
            Consts.Test_Key,
            token,
            out var claims);

        Assert.Multiple(() =>
        {
            Assert.That(token, Is.Not.Null);
            Assert.That(token, Is.Not.Empty);
        });
    }

    [Test]
    public void CanValidateATokenWithClaims()
    {
        var NAME_IDENTIFIER = "MIGUEL";
        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.NameIdentifier, NAME_IDENTIFIER),
        };
        var token = JwtSecurityTokenHelper.BuildToken(
            Consts.Test_Key,
            claims,
            1);

        var isValid = JwtSecurityTokenHelper.ValidateToken(
            Consts.Test_Key,
            token,
            out var claimsValidateToken);

        var nameIdentifierClaim = claimsValidateToken.First(x => x.Type == ClaimTypes.NameIdentifier);
        Assert.Multiple(() =>
        {
            Assert.That(token, Is.Not.Null);
            Assert.That(token, Is.Not.Empty);
            Assert.That(isValid, Is.True);
            Assert.That(nameIdentifierClaim.Value, Is.EqualTo(NAME_IDENTIFIER));
        });
    }
}
