using OperationResult;
using Refit;
using static OperationResult.Helpers;

namespace FreyaMobile.Features.Clients.ChangePass;

public class ChangePassService : IChangePassService
{
    private readonly IClientDataService userDataService;
    private readonly DialogService dialogService;
    private readonly ILogger<UserService> logger;

    public ChangePassService(
    IClientDataService userDataService,
    DialogService dialogService,
    ILogger<UserService> logger)
    {
        this.userDataService = userDataService;
        this.dialogService = dialogService;
        this.logger = logger;
    }

    public async Task<Result<bool>> ChangePassAsync(string newPass, string actualPass)
    {
        var userResponse = await userDataService.ChangePass(newPass, actualPass, OnChangePassErrorHandler);

        if (userResponse.IsError)
        {
            return Error();
        }

        return true;
    }

    private async Task<bool> OnChangePassErrorHandler(Exception exception)
    {
        if (exception is ApiException apiException && apiException.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            await dialogService.DisplayAlert("Compruebe los campos", string.Empty, "Cerrar");
            return true;
        }
        else
        {
            return false;
        }
    }
}
