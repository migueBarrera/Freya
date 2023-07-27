using OperationResult;

namespace FreyaMobile.Features.Clients.ChangePass;

public interface IChangePassService
{
    Task<Result<bool>> ChangePassAsync(string newPass, string actualPass);
}
