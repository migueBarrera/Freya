namespace Freya.Validations.Rules;

public class NotEmptyRule : IValidationRule<string>
{
    public NotEmptyRule(string validationMessage)
    {
        ValidationMessage = validationMessage;
    }

    public string ValidationMessage { get;set; } = string.Empty;

    public bool Check(string value)
    {
        return !string.IsNullOrEmpty(value);
    }
}
