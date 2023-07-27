namespace Freya.Validations.Rules;

public class NotEqualToZeroRule : IValidationRule<string>
{
    public NotEqualToZeroRule(string validationMessage)
    {
        ValidationMessage = validationMessage;
    }

    public string ValidationMessage { get; set; } = string.Empty;

    public bool Check(string value)
    {
        if (long.TryParse(value, out var result))
        {
            return result is not 0;
        }
        else
        {
            return false;
        }
    }
}
