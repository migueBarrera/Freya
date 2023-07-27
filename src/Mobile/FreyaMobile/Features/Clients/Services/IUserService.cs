using OperationResult;

namespace FreyaMobile.Features.Clients.Services;

public interface IUserService
{
    Task<Result<bool>> LogInUser(string email, string pass);
    Task<Result<bool>> TryRefreshUserAsync();
}
