using OperationResult;
using Refit;
using static OperationResult.Helpers;

namespace FreyaMobile.Features.Clients.Register;

public class RegisterClientService : IRegisterClientService
{
    private readonly IRegisterClientDataService registerClientDataService;
    private readonly DialogService dialogService;
    private readonly ILogger<UserService> logger;

    public RegisterClientService(
        IRegisterClientDataService userDataService,
        DialogService dialogService,
        ILogger<UserService> logger)
    {
        this.registerClientDataService = userDataService;
        this.dialogService = dialogService;
        this.logger = logger;
    }

    public async Task<Result<bool>> RegisterUser(RegisterUserValidatable registerUser)
    {
        var userResponse = await registerClientDataService.RegisterUser(registerUser, OnRegisterUserErrorHandler);

        if (userResponse.IsError)
        {
            return Error();
        }

        return true;
    }

    private async Task<bool> OnRegisterUserErrorHandler(Exception exception)
    {
        if (exception is ApiException apiException)
        {
            if (apiException.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                await dialogService.DisplayAlert("Compruebe los datos", string.Empty, "Cerrar");
                return true;
            }
            else if (apiException.StatusCode == System.Net.HttpStatusCode.Conflict)
            {
                await dialogService.DisplayAlert("Ese correo ya esta registrado", string.Empty, "Cerrar");
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
