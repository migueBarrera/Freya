namespace Freya.Validations.Rules;

public class MaxLengthRule : IValidationRule<string>
{
    private int maxLength;

    public MaxLengthRule(string message, int maxLength)
    {
        ValidationMessage = message;
        this.maxLength = maxLength;
    }

    public string ValidationMessage { get; set; }

    public bool Check(string value)
    {
        return value.Length < maxLength;
    }
}
