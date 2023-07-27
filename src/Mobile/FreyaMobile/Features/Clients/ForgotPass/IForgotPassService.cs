using OperationResult;

namespace FreyaMobile.Features.Clients.ForgotPass;

public interface IForgotPassService
{
    Task<Result<bool>> ForgorPassAsync(string value);
}
