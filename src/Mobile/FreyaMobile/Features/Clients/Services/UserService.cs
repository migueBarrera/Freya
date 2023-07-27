using OperationResult;
using Refit;
using static OperationResult.Helpers;

namespace FreyaMobile.Features.Clients.Services;

public class UserService : IUserService
{
    private readonly IClientDataService userDataService;
    private readonly DialogService dialogService;
    private readonly ILoggingService logger;

    public UserService(IClientDataService userDataService, DialogService dialogService, ILoggingService logger)
    {
        this.userDataService = userDataService;
        this.dialogService = dialogService;
        this.logger = logger;
    }

    public async Task<Result<bool>> LogInUser(string email, string pass)
    {
        var userResponse = await userDataService.LogInUser(email, pass, OnLogInUserErrorHandler);

        if (userResponse.IsError)
        {
            return Error();
        }

        return true;
    }

    public async Task<Result<bool>> TryRefreshUserAsync()
    {
        var userResponse = await userDataService.RefreshUser(OnRefreshUserErrorHandler);

        if (userResponse.IsError)
        {
            return Error();
        }

        return userResponse.IsSuccess;
    }

    private async Task<bool> OnLogInUserErrorHandler(Exception exception)
    {
        if (exception is ApiException apiException && apiException.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            await dialogService.DisplayAlert("Email o contraseña incorrectos", string.Empty);
            return true;
        }
        else
        {
            return false;
        }
    }

    private async Task<bool> OnRefreshUserErrorHandler(Exception exception)
    {
        if (exception is ApiException apiException && apiException.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            await dialogService.DisplayAlert("Algo salio mal", string.Empty);
            return true;
        }
        else
        {
            return false;
        }
    }
}
