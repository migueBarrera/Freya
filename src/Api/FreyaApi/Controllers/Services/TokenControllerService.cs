namespace FreyaApi.Controllers.Services;

public class TokenControllerService
{
    private readonly JwtSecurityTokenService jwtSecurityToken;

    public TokenControllerService(
        JwtSecurityTokenService jwtSecurityToken)
    {
        this.jwtSecurityToken = jwtSecurityToken;
    }

    public ActionResult<RefreshTokenResponse> RefresTokenClient(ControllerBase controller, RefreshTokenRequest request)
    {
        if (jwtSecurityToken.ValidateToken(request.RefreshToken, out var claims))
        {
            var response = new RefreshTokenResponse()
            {
                Token = jwtSecurityToken.BuildToken(claims),
                RefreshToken = jwtSecurityToken.BuildRefreshToken(claims),
            };

            return controller.Ok(response);
        }
        else
        {
            return controller.Unauthorized();
        }
    }
}
