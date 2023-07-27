using OperationResult;

namespace FreyaMobile.Features.Clients.Register;

public interface IRegisterClientDataService
{
    Task<Result<Client>> RegisterUser(
        RegisterUserValidatable registerUser,
        Func<Exception, Task<bool>> errorHandler);
}
