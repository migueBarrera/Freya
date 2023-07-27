namespace FreyaManager.Features.SignIn
{
    public interface ISignInService
    {
        Task<bool> SignInAsync(string email, string pass);
    }
}
