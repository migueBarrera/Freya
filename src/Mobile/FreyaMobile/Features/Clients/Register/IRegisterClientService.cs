using OperationResult;

namespace FreyaMobile.Features.Clients.Register;

public interface IRegisterClientService
{
    Task<Result<bool>> RegisterUser(RegisterUserValidatable registerUser);
}
