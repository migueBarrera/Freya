using OperationResult;

namespace FreyaMobile.Features.Clients.Services;

public interface IClientDataService
{
    Task<Result<Client>> LogInUser(
        string email,
        string pass,
        Func<Exception, Task<bool>> errorHandler);

    Task<Result<Client>> RefreshUser(
        Func<Exception, Task<bool>> errorHandler);

    Task<Result<bool>> ForgotPass(
        string email,
        Func<Exception, Task<bool>> errorHandler);

    Task<Result<bool>> ChangePass(
        string newPass, 
        string actualPass, 
        Func<Exception, Task<bool>> errorHandler);

    Task<Result<bool>> UpdateUser(
        ClientUpdateRequest request,
        Func<Exception, Task<bool>> errorHandler);
}
