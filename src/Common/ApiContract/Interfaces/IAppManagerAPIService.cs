using Models.Core.AppManagers;

namespace ApiContract.Interfaces;

public interface IAppManagerAPIService
{
    [Post("/api/AppManager/v1/SignIn")]
    Task<AppManagerSignInResponse> SignInAsync([Body] AppManagerSignInRequest request);
}
