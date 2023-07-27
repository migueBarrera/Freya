using OperationResult;

namespace FreyaMobile.Features.Clients.Services;

public interface IEditClientService
{
    Task<Result<bool>> UpdateUser(UpdateUserValidatable updateUser);
}
