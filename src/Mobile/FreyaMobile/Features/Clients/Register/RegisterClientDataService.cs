using ApiContract.Interfaces;
using OperationResult;
using static OperationResult.Helpers;

namespace FreyaMobile.Features.Clients.Register;

public class RegisterClientDataService : IRegisterClientDataService
{
    private readonly IClientAPIService userApi;
    private readonly ITaskHelperFactory taskHelperFactory;
    private readonly ILoggingService logger;
    private readonly IAuthTokenService authTokenService;
    private readonly ICurrentUserService currentUserService;

    public RegisterClientDataService(
        IRefitService refitService,
        ITaskHelperFactory taskHelperFactory,
        ILoggingService logger,
        ICurrentUserService currentUserService,
        IAuthTokenService authTokenService)
    {
        this.userApi = refitService.InitRefitInstance<IClientAPIService>(isAutenticated: false);
        this.taskHelperFactory = taskHelperFactory;
        this.logger = logger;
        this.currentUserService = currentUserService;
        this.authTokenService = authTokenService;
    }

    public async Task<Result<Client>> RegisterUser(
    RegisterUserValidatable registerUser,
    Func<Exception, Task<bool>> errorHandler)
    {
        Result<Client> result = Error();
        var request = new ClientSignUpRequest()
        {
            Email = registerUser.Email.Value,
            LastName = registerUser.LastName.Value,
            Name = registerUser.Name.Value,
            Pass = registerUser.Pass.Value,
            Phone = registerUser.Phone.Value,
        };

        var response = await this.taskHelperFactory
            .CreateInternetAccessViewModelInstance(this.logger)
            .WithErrorHandling(errorHandler)
            .TryExecuteAsync(() => userApi.SignUpAsync(request));

        if (response.IsSuccess)
        {
            var user = new Client()
            {
                Id = response.Value.Id,
                Email = response.Value.Email,
                Name = response.Value.Name,
                LastName = response.Value.LastName,
                Phone = response.Value.Phone,
                //PregnancyDate = response.Value.PregnancyDate,
                Clinics = Enumerable.Empty<Clinic>(),
            };

            await authTokenService.SetToken(response.Value.Token, string.Empty);
            await currentUserService.SetUserAsync(user);

            result = Ok(user);
        }

        return result;
    }
}
