namespace Freya.Validations.Rules;

public class EmailValidationRule : IValidationRule<string>
{
    public EmailValidationRule(string message)
    {
        ValidationMessage = message;
    }

    public string ValidationMessage { get; set; }

    public bool Check(string value)
    {
        return ValidateHelper.IsValidEmail(value);
    }
}
