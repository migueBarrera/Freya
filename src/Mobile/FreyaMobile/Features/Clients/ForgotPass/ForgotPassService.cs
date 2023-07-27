using OperationResult;
using Refit;
using static OperationResult.Helpers;

namespace FreyaMobile.Features.Clients.ForgotPass;

public class ForgotPassService : IForgotPassService
{
    private readonly IClientDataService userDataService;
    private readonly DialogService dialogService;
    private readonly ILogger<ForgotPassService> logger;

    public ForgotPassService(
        IClientDataService userDataService,
        DialogService dialogService,
        ILogger<ForgotPassService> logger)
    {
        this.userDataService = userDataService;
        this.dialogService = dialogService;
        this.logger = logger;
    }

    public async Task<Result<bool>> ForgorPassAsync(string email)
    {
        var userResponse = await userDataService.ForgotPass(email, OnForgotPassErrorHandler);

        if (userResponse.IsError)
        {
            return Error();
        }

        return true;
    }

    private async Task<bool> OnForgotPassErrorHandler(Exception exception)
    {
        if (exception is ApiException apiException && apiException.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            await dialogService.DisplayAlert("Ese email no esta registrado", string.Empty, "Cerrar");
            return true;
        }
        else
        {
            return false;
        }
    }
}
