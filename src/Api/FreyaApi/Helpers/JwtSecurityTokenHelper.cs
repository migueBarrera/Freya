namespace FreyaApi.Helpers;

public static class JwtSecurityTokenHelper
{
    public static string BuildToken(
        string jwtKey,
        IEnumerable<Claim> claims,
        int expireDays)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(jwtKey);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(expireDays),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }

    public static bool ValidateToken(string JwtKey, string authToken, out IEnumerable<Claim> claims)
    {
        claims = Enumerable.Empty<Claim>();

        var tokenHandler = new JwtSecurityTokenHandler();
        var validationParameters = GetValidationParameters(Encoding.ASCII.GetBytes(JwtKey));

        SecurityToken validatedToken;
        try
        {
            ClaimsPrincipal principal = tokenHandler.ValidateToken(authToken, validationParameters, out validatedToken);
            claims = principal.Claims;
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public static TokenValidationParameters GetValidationParameters(byte[] key)
    {
        return new TokenValidationParameters()
        {
            ValidateLifetime = true, // Because there is no expiration in the generated token
            ValidateAudience = false, // Because there is no audiance in the generated token
            ValidateIssuer = false,   // Because there is no issuer in the generated token
            ValidIssuer = "Sample",
            ValidAudience = "Sample",
            IssuerSigningKey = new SymmetricSecurityKey(key) // The same key as the one that generate the token
        };
    }

    public static string DetermineEmployeeRol(EmployeeRol rol)
    {
        return rol switch
        {
            EmployeeRol.COMPANY_OWNER => SystemRoles.COMPANY_OWNER,
            EmployeeRol.COMPANY_MANAGER => SystemRoles.COMPANY_MANAGER,
            EmployeeRol.CLINIC_MANAGER => SystemRoles.CLINIC_MANAGER,
            EmployeeRol.CLINIC_OFFICER => SystemRoles.CLINIC_OFFICER,
            _ => SystemRoles.CLINIC_OFFICER,
        };
    }
}
