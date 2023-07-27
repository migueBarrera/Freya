namespace MiBebeClient.Features.Clients;

public class ClientSignInValidatable : BaseValidatable
{
    private const int MIN_PASS_LENGTH = 4;

    public ClientSignInValidatable()
    {
        Email = new ValidatableObject<string>() { Value = string.Empty, };
        Pass = new ValidatableObject<string>() { Value = string.Empty, };

        Email.Validations.Add(new EmailValidationRule("Revise el email"));
        Pass.Validations.Add(new MinLengthRule($"La contraseña debe ser de {MIN_PASS_LENGTH} caracteres", MIN_PASS_LENGTH));
    }

    public ValidatableObject<string> Pass { get; private set; } = new ValidatableObject<string>();

    public ValidatableObject<string> Email { get; private set; } = new ValidatableObject<string>();


    public override IEnumerable<ValidatableObject<string>> ValidatablesProperties()
    {
        return new ValidatableObject<string>[] { Email, Pass };
    }
}
