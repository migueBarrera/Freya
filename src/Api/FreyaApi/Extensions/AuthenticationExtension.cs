namespace FreyaApi.Extensions;

public static class AuthenticationExtension
{
    public static IServiceCollection AddTokenAuthentication(this IServiceCollection services, IConfiguration config)
    {
        var secret = config.GetSection("Jwt").GetSection("Key").Value;

        ArgumentNullException.ThrowIfNull(secret);

        TokenValidationParameters tokenValidationParameters = JwtSecurityTokenHelper.GetValidationParameters(Encoding.ASCII.GetBytes(secret));

        services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(x =>
        {
            x.TokenValidationParameters = tokenValidationParameters;
        });

        return services;
    }
}
