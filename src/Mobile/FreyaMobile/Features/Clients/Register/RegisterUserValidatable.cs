namespace FreyaMobile.Features.Clients.Register;

public class RegisterUserValidatable : BaseValidatable
{
    private const int MIN_PASS_LENGTH = 4;

    public RegisterUserValidatable()
    {
        Email = new ValidatableObject<string>() { Value = string.Empty, };
        Pass = new ValidatableObject<string>() { Value = string.Empty, };
        RepeatPass = new ValidatableObject<string>() { Value = string.Empty, };
        Name = new ValidatableObject<string>() { Value = string.Empty, };
        LastName = new ValidatableObject<string>() { Value = string.Empty, };
        Phone = new ValidatableObject<string>() { Value = string.Empty, };

        Email.Validations.Add(new EmailValidationRule("Revise el email"));
        Pass.Validations.Add(new MinLengthRule($"La contraseña debe ser de {MIN_PASS_LENGTH} caracteres", MIN_PASS_LENGTH));
        RepeatPass.Validations.Add(new MinLengthRule(string.Empty, MIN_PASS_LENGTH));
    }

    public ValidatableObject<string> Email { get; private set; } = new ValidatableObject<string>();

    public ValidatableObject<string> LastName { get; private set; } = new ValidatableObject<string>();

    public ValidatableObject<string> Name { get; private set; } = new ValidatableObject<string>();

    public ValidatableObject<string> Phone { get; private set; } = new ValidatableObject<string>();

    public ValidatableObject<string> Pass { get; private set; } = new ValidatableObject<string>();

    public ValidatableObject<string> RepeatPass { get; private set; } = new ValidatableObject<string>();

    public int Weeks { get; set; }

    public override IEnumerable<ValidatableObject<string>> ValidatablesProperties()
    {
        return new ValidatableObject<string>[] { Name, Phone, Email, Pass, LastName, RepeatPass };
    }
}
