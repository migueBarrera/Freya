namespace FreyaApi.Controllers.Services;

public class AppManagerControllerService
{
    private readonly JwtSecurityTokenService jwtSecurityToken;
    private readonly IAppManagerRepository appManagerRepository;

    public AppManagerControllerService(JwtSecurityTokenService jwtSecurityToken, IAppManagerRepository appManagerRepository)
    {
        this.jwtSecurityToken = jwtSecurityToken;
        this.appManagerRepository = appManagerRepository;
    }

    public ActionResult<AppManagerSignInResponse> PostSignIn(ControllerBase controller, AppManagerSignInRequest request)
    {
        AppManagerTable? appManager = appManagerRepository.GetAppManager(request.Email);

        if (appManager == null)
        {
            return controller.NotFound();
        }

        if (!Hash.Validate(request.Pass, appManager.Pass))
        {
            return controller.NotFound();
        }

        var response = new AppManagerSignInResponse()
        {
            Id = appManager.Id,
            Email = appManager.Email,
            Token = jwtSecurityToken.BuildToken(appManager),
            RefreshToken = jwtSecurityToken.BuildRefreshToken(appManager),
        };

        return controller.Ok(response);
    }
}
