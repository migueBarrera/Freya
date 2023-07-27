using ApiContract.Interfaces;
using Refit;

namespace FreyaManager.Features.SignIn;

public class SignInService : ISignInService
{
    private readonly IAppManagerAPIService appManagerAPIService;
    private readonly ITaskHelperFactory taskHelperFactory;
    private readonly IDialogService dialogService;
    private readonly ICurrentAppManagerService currentAppManagerService;
    private readonly IAuthTokenService authTokenService;
    private readonly ILogger<SignInService> logger;

    public SignInService(
        IRefitService refitService,
        ITaskHelperFactory taskHelperFactory,
        ILogger<SignInService> logger,
        IDialogService dialogService,
        IAuthTokenService authTokenService,
        ICurrentAppManagerService currentAppManagerService)
    {
        appManagerAPIService = refitService.InitRefitInstance<IAppManagerAPIService>(isAutenticated: false);
        this.taskHelperFactory = taskHelperFactory;
        this.logger = logger;
        this.dialogService = dialogService;
        this.authTokenService = authTokenService;
        this.currentAppManagerService = currentAppManagerService;
    }

    public async Task<bool> SignInAsync(string email, string pass)
    {
        var result = await taskHelperFactory.
                                CreateInternetAccessViewModelInstance(logger).
                                WithErrorHandling(OnSignInError).
                                TryExecuteAsync(
                                () => appManagerAPIService.SignInAsync(new Models.Core.AppManagers.AppManagerSignInRequest()
                                {
                                    Email = email,
                                    Pass = pass,
                                }));
        if (result.IsSuccess)
        {
            var value = result.Value;
            await currentAppManagerService.Set(new Models.Core.AppManagers.AppManager()
            {
                Id = value.Id,
                Email = value.Email,
            });
            await authTokenService.SetToken(value.Token, value.Token);
        }

        return result.IsSuccess;
    }

    private async Task<bool> OnSignInError(Exception arg)
    {
        if (arg is ApiException apiException && apiException.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            await dialogService.ShowMessage("Error", "Email o pass incorrectos");
            return true;
        }

        return false;
    }
}
