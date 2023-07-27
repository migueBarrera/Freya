namespace FreyaApi.Services;

public class JwtSecurityTokenService
{
    private readonly string jwtToken;

    private const int EXPIRES_DAYS_TOKEN = 1;
    private const int EXPIRES_DAYS_REFRESH_TOKEN = 31;

    public JwtSecurityTokenService(IConfiguration configuration)
    {
        var token = configuration.GetValue<string>("Jwt:Key");

        ArgumentNullException.ThrowIfNull(token, nameof(token));

        jwtToken = token;
    }

    public string BuildToken(
        ClientTable client)
    {
        return BuildToken(
            new[]
            {
                new Claim(ClaimTypes.Email, client.Email),
                new Claim(ClaimTypes.NameIdentifier, client.Id.ToString()),
                new Claim(ClaimTypes.Role, SystemRoles.CLIENT_MOBILE),
            });
    }

    public string BuildToken(
        AppManagerTable manager)
    {
        return BuildToken(
            new[]
            {
            new Claim(ClaimTypes.Email, manager.Email),
            new Claim(ClaimTypes.NameIdentifier, manager.Id.ToString()),
            new Claim(ClaimTypes.Role, SystemRoles.APP_MANAGER),
            });
    }

    public string BuildToken(
        EmployeeTable employee)
    {
        return BuildToken(
            new[]
            {
            new Claim(ClaimTypes.Email, employee.Email),
            new Claim(ClaimTypes.NameIdentifier, employee.Id.ToString()),
            new Claim(ClaimTypes.Role, JwtSecurityTokenHelper.DetermineEmployeeRol(employee.Rol)),
            });
    }

    public string BuildRefreshToken(
      ClientTable client)
    {
        return BuildRefreshToken(
            new[]
            {
                new Claim(ClaimTypes.Email, client.Email),
                new Claim(ClaimTypes.NameIdentifier, client.Id.ToString()),
                new Claim(ClaimTypes.Role, SystemRoles.CLIENT_MOBILE),
            });
    }

    public string BuildRefreshToken(
        AppManagerTable manager)
    {
        return BuildRefreshToken(
            new[]
            {
                new Claim(ClaimTypes.Email, manager.Email),
                new Claim(ClaimTypes.NameIdentifier, manager.Id.ToString()),
                new Claim(ClaimTypes.Role, SystemRoles.APP_MANAGER),
            });
    }

    public string BuildRefreshToken(
        EmployeeTable employee)
    {
        return BuildRefreshToken(
            new[]
            {
                new Claim(ClaimTypes.Email, employee.Email),
                new Claim(ClaimTypes.NameIdentifier, employee.Id.ToString()),
                new Claim(ClaimTypes.Role, JwtSecurityTokenHelper.DetermineEmployeeRol(employee.Rol)),
            });
    }

    public string BuildToken(
        IEnumerable<Claim> claims)
    {
        return JwtSecurityTokenHelper.BuildToken(jwtToken, claims, EXPIRES_DAYS_TOKEN);
    }

    public string BuildRefreshToken(
        IEnumerable<Claim> claims)
    {
        return JwtSecurityTokenHelper.BuildToken(jwtToken, claims, EXPIRES_DAYS_REFRESH_TOKEN);
    }

    public bool ValidateToken(string refreshToken, out IEnumerable<Claim> claims)
    {
        if (JwtSecurityTokenHelper.ValidateToken(jwtToken, refreshToken, out claims))
        {
            var claimNameIdentifier = claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            if (claimNameIdentifier != null && Guid.TryParse(claimNameIdentifier.Value, out var id))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
}
