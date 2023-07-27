namespace FreyaApi.Helpers;

public static class UserControllerHelper
{
    public static Guid GetLoggedUserId(this ControllerBase controller)
    {
        bool isAutenticated = controller.User?.Identity?.IsAuthenticated ?? false;
        if (!isAutenticated)
            throw new AuthenticationException();

        string userId = controller.User?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? string.Empty;

        return Guid.Parse(userId);
    }
}
