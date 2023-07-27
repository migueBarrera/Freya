namespace Freya.Features.SignIn;

public interface ISignInService
{
    Task<bool> SignInAsync(string email, string pass);
    Task<bool> RecoverPass(string email);
}
