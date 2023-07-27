namespace FreyaMobile.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly ICurrentUserService currentUserService;
     
    public AuthenticationService(ICurrentUserService currentUserService)
    {
        this.currentUserService = currentUserService;
    }

    public async Task SignOut()
    {
        await currentUserService.SetUserAsync(null);
        await AppShell.Current.GoToAsync($"///Home");
    }
}
