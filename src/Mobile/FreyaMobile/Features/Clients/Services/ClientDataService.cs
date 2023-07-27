using ApiContract.Interfaces;
using OperationResult;
using static OperationResult.Helpers;

namespace FreyaMobile.Features.Clients.Services;

public class ClientDataService : IClientDataService
{
    private readonly IClientAPIService userApi;
    private readonly ITaskHelperFactory taskHelperFactory;
    private readonly ILoggingService logger;
    private readonly IAuthTokenService authTokenService;
    private readonly ICurrentUserService currentUserService;

    public ClientDataService(
        IRefitService refitService,
        ITaskHelperFactory taskHelperFactory,
        ILoggingService logger,
        ICurrentUserService currentUserService,
        IAuthTokenService authTokenService)
    {
        this.userApi = refitService.InitRefitInstance<IClientAPIService>(isAutenticated: true);
        this.taskHelperFactory = taskHelperFactory;
        this.logger = logger;
        this.currentUserService = currentUserService;
        this.authTokenService = authTokenService;
    }

    public async Task<Result<bool>> ChangePass(string newPass, string actualPass, Func<Exception, Task<bool>> errorHandler)
    {
        Result<Client> result = Error();
        var user = await currentUserService.GetCurrentUserAsync();
        var request = new ClientChangePassRequest()
        {
            Password = newPass,
            ClientId = user!.Id,
            ActualPassword = actualPass,
        };

        var response = await this.taskHelperFactory
            .CreateInternetAccessViewModelInstance(null)
            .WithErrorHandling(errorHandler)
            .TryExecuteAsync(() => userApi.ChangePass(request));

        if (response.IsSuccess)
        {
            return true;
        }
        else
        {
            return Error();
        }
    }

    public async Task<Result<bool>> ForgotPass(
        string email,
        Func<Exception, Task<bool>> errorHandler)
    {
        Result<bool> result = Error();
        var request = new ClientForgotPassRequest()
        {
            Email = email
        };

        var response = await this.taskHelperFactory
            .CreateInternetAccessViewModelInstance(null)
            .WithErrorHandling(errorHandler)
            .TryExecuteAsync(() => userApi.ForgotPass(request));

        if (response.IsSuccess)
        {
            return true;
        }
        else
        {
            return Error();
        }
    }

    public async Task<Result<Client>> LogInUser(string email, string pass, Func<Exception, Task<bool>> errorHandler)
    {
        Result<Client> result = Error();
        var request = new ClientSignInRequest()
        {
            Email = email,
            Pass = pass,
        };

        var response = await this.taskHelperFactory
            .CreateInternetAccessViewModelInstance(this.logger)
            .WithErrorHandling(errorHandler)
            .TryExecuteAsync(() => userApi.SignInAsync(request));

        if (response.IsSuccess)
        {
            var user = new Client()
            {
                Id = response.Value.Id,
                Email = response.Value.Email,
                Name = response.Value.Name,
                LastName = response.Value.LastName,
                Clinics = response.Value.Clinics,
                Phone = response.Value.Phone,
                //PregnancyDate = response.Value.PregnancyDate,
            };

            await authTokenService.SetToken(response.Value.Token, response.Value.RefreshToken);
            await currentUserService.SetUserAsync(user);
            if (user.Clinics?.Any() == true)
            {
                await currentUserService.SetClinicSelectedAsync(user.Clinics.First());
            }

            result = Ok(user);
        }

        return result;
    }

    public async Task<Result<Client>> RefreshUser(Func<Exception, Task<bool>> errorHandler)
    {
        Result<Client> result = Error();

        var isLogged = await currentUserService.IsLoggedAsync();
        if (!isLogged)
        {
            logger.Debug("User not logged, skiping refresh...");
            return result;
        }

        var currentUser = await currentUserService.GetCurrentUserAsync();

        var response = await this.taskHelperFactory
            .CreateInternetAccessViewModelInstance(this.logger)
            .WithErrorHandling(errorHandler)
            .TryExecuteAsync(() => userApi.RefreshClient(currentUser!.Id));

        if (response.IsSuccess)
        {
            var user = new Client()
            {
                Id = response.Value.Id,
                Email = response.Value.Email,
                Name = response.Value.Name,
                LastName = response.Value.LastName,
                Clinics = response.Value.Clinics,
                Phone = response.Value.Phone,
                //PregnancyDate = response.Value.PregnancyDate,
            };

            await authTokenService.SetToken(response.Value.Token, string.Empty);
            await currentUserService.SetUserAsync(user);
            if (user.Clinics?.Any() == true)
            {
                await currentUserService.SetClinicSelectedAsync(user.Clinics.First());
            }

            result = Ok(user);
        }

        return result;
    }

    public async Task<Result<bool>> UpdateUser(ClientUpdateRequest request, Func<Exception, Task<bool>> errorHandler)
    {
        Result<bool> result = Error();
        var user = await currentUserService.GetCurrentUserAsync();

        var response = await this.taskHelperFactory
            .CreateInternetAccessViewModelInstance(null)
            .WithErrorHandling(errorHandler)
            .TryExecuteAsync(() => userApi.UpdateClient(user!.Id, request));

        if (response.IsSuccess)
        {
            user!.Name = request.Name;
            user.LastName = request.LastName;
            user.Phone = request.Phone;
            user.PregnancyDate = request.PregnancyDate;
            await currentUserService.SetUserAsync(user);
            return true;
        }
        else
        {
            return Error();
        }
    }

}

