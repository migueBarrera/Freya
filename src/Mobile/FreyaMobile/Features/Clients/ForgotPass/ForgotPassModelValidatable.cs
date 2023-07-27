namespace FreyaMobile.Features.Clients.ForgotPass;

public class ForgotPassModelValidatable : BaseValidatable
{
    public ForgotPassModelValidatable()
    {
        this.Email = new ValidatableObject<string>() { Value = string.Empty, };

        Email.Validations.Add(new EmailValidationRule("Revise el email"));
    }

    public ValidatableObject<string> Email { get; private set; } = new ValidatableObject<string>();

    public void SetEmail(string email)
    {
        this.Email.Value = email;
    }

    public override IEnumerable<ValidatableObject<string>> ValidatablesProperties()
    {
        return new ValidatableObject<string>[] { this.Email };
    }
}
